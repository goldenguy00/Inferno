using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MonoMod.Cil;
using R2API;
using R2API.Utils;
using Rewired.ComponentControls.Effects;
using RoR2;
using RoR2.Achievements;
using RoR2.CharacterAI;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using EntityStates.BrotherMonster.Weapon;
using Inferno.Skill_Misc;
using Inferno.Stat_AI;
using Inferno.Item;

namespace Inferno
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.RiskyLives.RiskyMod", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [R2APISubmoduleDependency(nameof(DifficultyAPI), nameof(LanguageAPI), nameof(RecalculateStatsAPI), nameof(ItemAPI))]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "Inferno";
        public const string PluginVersion = "1.5.1";

        public static DifficultyDef InfernoDiffDef;

        public static DifficultyIndex InfernoDiffIndex;

        public AssetBundle inferno;

        public static ManualLogSource InfernoLogger;

        public static bool check;

        public static AISkillDriver BrotherHurtDash;

        public static AISkillDriver BrotherHurtLeap;

        public static List<AISkillDriver> hurtstuff = new();

        public static ConfigEntry<float> Scaling { get; set; }
        public static ConfigEntry<float> LevelRegen { get; set; }
        public static ConfigEntry<float> LevelMoveSpeed { get; set; }
        public static ConfigEntry<float> LevelAttackSpeed { get; set; }
        public static ConfigEntry<float> LoopArmor { get; set; }
        public static ConfigEntry<float> BossHp { get; set; }
        public static ConfigEntry<float> MonsterHp { get; set; }
        public static ConfigEntry<float> StageCooldownReduction { get; set; }
        public static ConfigEntry<float> LevelDiffBoost { get; set; }
        public static ConfigEntry<bool> LevelDiffBoostScaleWithPlayers { get; set; }
        public static ConfigEntry<float> AllyPermanentDamage { get; set; }
        public static ConfigEntry<float> ProjectileSpeed { get; set; }
        public static ConfigEntry<int> MonsterLimit { get; set; }

        public static ConfigEntry<bool> EnableAI { get; set; }
        public static ConfigEntry<float> AIScaling { get; set; }
        public static ConfigEntry<bool> EnableSkills { get; set; }
        public static ConfigEntry<bool> EnableStats { get; set; }

        public static ConfigEntry<bool> EnableCDirector { get; set; }
        public static ConfigEntry<float> CreditMultiplier { get; set; }

        public static ConfigEntry<float> TeleporterSpeed { get; set; }
        public static ConfigEntry<float> TeleporterSize { get; set; }

        public static ConfigEntry<float> ColorGradingRedGain { get; set; }
        public static ConfigEntry<float> VignetteIntensity { get; set; }
        public static ConfigEntry<float> PostProcessingWeight { get; set; }

        public static ConfigEntry<bool> MithrixAS { get; set; }

        public static ConfigEntry<bool> Important { get; set; }

        public static ColorGrading cg;
        public static Vignette vn;

        public static bool ShouldRun = false;
        public static GameObject ppHolder;

        public static bool hasFired;

        public static UnlockableDef CommandoSkin;
        public static UnlockableDef BanditSkin;
        public static UnlockableDef CaptainSkin;

        private static readonly System.Random random = new();

        private static readonly System.Random rnggggggggggggg = new();

        private static readonly System.Random randomAttackSpeedFuckYou = new();

        public static GameObject Ramp1;
        public static GameObject Ramp2;
        public static GameObject Ramp3;
        public static GameObject Rocks;

        public static int ShardCount = 0;

        public void Awake()
        {
            Main.InfernoLogger = base.Logger;

            inferno = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("Inferno.dll", "inferno"));

            Items.Create();

            Important = Config.Bind("General", "! Important !", true, "Make sure everyone's configs are the same for multiplayer!");
            Scaling = Config.Bind("General", "Difficulty Scaling", 150f, "Percentage of difficulty scaling, 150% is +50%. Difficulty order does not update visually. Vanilla is 150");
            LevelMoveSpeed = Config.Bind("General", "Enemy Move Speed Scaling", 0.12f, "Adds move speed to each monster every level. Vanilla is 0");
            LevelRegen = Config.Bind("General", "Enemy Regen Scaling", 0.12f, "Adds health regen to each monster every level. Vanilla is 0");
            LevelAttackSpeed = Config.Bind("General", "Enemy Attack Speed Scaling", 0.003f, "Adds attack speed to each monster every level. Vanilla is 0");
            LoopArmor = Config.Bind("General", "Enemy Armor Scaling", 10f, "Adds armor to each monster every completed loop. Vanilla is 0");
            MonsterHp = Config.Bind("General", "Enemy Health Scaling", 0.06f, "Adds % max hp to each monster every completed stage. Vanilla is 0");
            BossHp = Config.Bind("General", "Boss Health Scaling", 0.07f, "Adds % max hp to each boss every completed stage. Vanilla is 0");
            StageCooldownReduction = Config.Bind("General", "Enemy Cooldown Reduction Scaling", 0.01f, "Adds % cooldown reduction to each monster every completed stage. Vanilla is 0");
            ProjectileSpeed = Config.Bind("General II", "Enemy Projectile Speed", 1.25f, "Sets the projectile speed multiplier. Vanilla is 1");
            LevelDiffBoost = Config.Bind("General II", "Level and Difficulty Boost", 1.25f, "Adds to ambient level and difficulty coefficient. Vanilla is 0");
            LevelDiffBoostScaleWithPlayers = Config.Bind("General II", "Scale Difficulty Boost with player count?", false, "Vanilla is false");
            AllyPermanentDamage = Config.Bind("General III", "Ally Permanent Damage", 0f, "Makes allies take % max hp permanent damage. Vanilla is 0, Eclipse 8 is 40");
            MonsterLimit = Config.Bind("General II", "Enemy Cap", 60, "Sets the enemy cap. Vanilla is 40");
            EnableAI = Config.Bind("General II", "Enable AI Changes?", true, "Vanilla is false");
            AIScaling = Config.Bind("General II", "AI Scaling Coefficient", 1.0f, "Adds to AI aim and range every cleared stage. Only works with AI Changes enabled. Vanilla is false");
            EnableSkills = Config.Bind("General II", "Enable Skill Changes?", true, "Vanilla is false");
            EnableStats = Config.Bind("General II", "Enable Stat Changes?", true, "Vanilla is false");
            EnableCDirector = Config.Bind("General II", "Enable Combat Director Changes?", true, "Makes the combat director spawn monsters closer and with more variety during looping. Vanilla is false");
            CreditMultiplier = Config.Bind("General II", "C. Director Credit Multiplier", 0.05f, "Adds to the combat director credit multiplier every cleared stage. Vanilla is 0");
            TeleporterSpeed = Config.Bind("General III", "Holdout Zone Speed", 30f, "Adds to the percent of holdout zone speed. Vanilla is 0");
            TeleporterSize = Config.Bind("General III", "Holdout Zone Radius", -30f, "Adds to the percent of holdout zone radius. Vanilla is 0");
            MithrixAS = Config.Bind("Bullshit", "Make Mithrix scale with Attack Speed?", false, "Vanilla is false");

            ColorGradingRedGain = Config.Bind("Post Processing", "Red Gain", 3.7f, "Vanilla is 0");
            VignetteIntensity = Config.Bind("Post Processing", "Vignette Intensity", 0.21f, "Vanilla is 0");
            PostProcessingWeight = Config.Bind("Post Processing", "Strength", 1f, "Vanilla is 0");

            AddDifficulty();
            FillTokens();
            PostProcessingSetup.Gupdate();

            ModSettingsManager.SetModIcon(inferno.LoadAsset<Sprite>("texInferno2Icon.png"));
            ModSettingsManager.AddOption(new GenericButtonOption("! Important ! ", "General", "Make sure everyone's configs are the same for multiplayer!", "                                      ", ResetConfig));
            ModSettingsManager.AddOption(new StepSliderOption(Scaling, new StepSliderConfig() { increment = 25f, min = 0f, max = 1600f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelAttackSpeed, new StepSliderConfig() { increment = 0.001f, min = 0f, max = 0.1f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelMoveSpeed, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelRegen, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(StageCooldownReduction, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(LoopArmor, new StepSliderConfig() { increment = 5f, min = 0f, max = 50f }));
            ModSettingsManager.AddOption(new StepSliderOption(MonsterHp, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 0.5f }));
            ModSettingsManager.AddOption(new StepSliderOption(BossHp, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 0.5f }));

            ModSettingsManager.AddOption(new StepSliderOption(ProjectileSpeed, new StepSliderConfig() { increment = 0.05f, min = 1f, max = 5f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelDiffBoost, new StepSliderConfig() { increment = 0.25f, min = 0f, max = 10f }));
            ModSettingsManager.AddOption(new CheckBoxOption(LevelDiffBoostScaleWithPlayers));
            ModSettingsManager.AddOption(new CheckBoxOption(EnableCDirector));
            ModSettingsManager.AddOption(new StepSliderOption(CreditMultiplier, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 0.25f }));
            ModSettingsManager.AddOption(new CheckBoxOption(EnableAI, new CheckBoxConfig() { description = "If in a run, restart it to apply changes. Vanilla is false" }));
            ModSettingsManager.AddOption(new StepSliderOption(AIScaling, new StepSliderConfig() { increment = 0.1f, min = 0f, max = 3f }));
            ModSettingsManager.AddOption(new CheckBoxOption(EnableSkills, new CheckBoxConfig() { description = "If in a run, restart it to apply changes. Vanilla is false" }));
            ModSettingsManager.AddOption(new CheckBoxOption(EnableStats, new CheckBoxConfig() { description = "If in a run, restart it to apply changes. Vanilla is false" }));
            ModSettingsManager.AddOption(new IntSliderOption(MonsterLimit, new IntSliderConfig() { min = 0, max = 200 }));

            ModSettingsManager.AddOption(new StepSliderOption(AllyPermanentDamage, new StepSliderConfig() { increment = 5f, min = 0f, max = 500f }));
            ModSettingsManager.AddOption(new StepSliderOption(TeleporterSpeed, new StepSliderConfig() { increment = 5f, min = -50f, max = 50f }));
            ModSettingsManager.AddOption(new StepSliderOption(TeleporterSize, new StepSliderConfig() { increment = 5f, min = -50f, max = 50f }));

            ModSettingsManager.AddOption(new GenericButtonOption("", "General", "Note that upon hitting the Reset to default button, this menu does not visually update until you leave the settings and go back in.", "Reset to default", ResetConfig));
            ModSettingsManager.AddOption(new CheckBoxOption(MithrixAS, new CheckBoxConfig() { category = "Bullshit" }));
            ModSettingsManager.AddOption(new GenericButtonOption("", "Bullshit", "Random bullshit go", "Randomize Config", RandomizeConfig));

            ModSettingsManager.AddOption(new StepSliderOption(ColorGradingRedGain, new StepSliderConfig() { increment = 0.1f, min = 0f, max = 20f }));
            ModSettingsManager.AddOption(new StepSliderOption(VignetteIntensity, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(PostProcessingWeight, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
            ModSettingsManager.AddOption(new GenericButtonOption("", "Post Processing", "Note that upon hitting the Reset to default button, this menu does not visually update until you leave the settings and go back in.", "Reset to default", ResetPostProcessing));

            var uselessPieceOfShit = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Beetle/BeetleBodySleep.asset").WaitForCompletion();
            uselessPieceOfShit.baseMaxStock = 0;
            uselessPieceOfShit.stockToConsume = 69;
            uselessPieceOfShit.requiredStock = 69;

            // Thanks Mystic ! ! !
            On.RoR2.Language.GetLocalizedStringByToken += Language_GetLocalizedStringByToken;

            Run.onRunSetRuleBookGlobal += ChangeAmbientCap;
            Run.onRunStartGlobal += (Run run) =>
            {
                ShouldRun = false;
                if (run.selectedDifficulty == InfernoDiffIndex)
                {
                    if (EnableStats.Value)
                    {
                        CharacterBody.onBodyAwakeGlobal += Body.BodyChanges;
                        RecalculateStatsAPI.GetStatCoefficients += Hooks.RecalculateStatsAPI_GetStatCoefficients;
                    }
                    if (EnableAI.Value)
                    {
                        CharacterMaster.onStartGlobal += Master.MasterChanges;
                        IL.EntityStates.AI.Walker.LookBusy.OnEnter += Hooks.LookBusy_OnEnter;
                        IL.EntityStates.AI.Walker.LookBusy.PickNewTargetLookDirection += Hooks.LookBusy_PickNewTargetLookDirection;
                        IL.EntityStates.AI.Walker.Wander.PickNewTargetLookPosition += Hooks.Wander_PickNewTargetLookPosition;
                    }
                    if (EnableSkills.Value)
                    {
                        HookBehavior.ApplyHooks();
                    }
                    IL.RoR2.Run.RecalculateDifficultyCoefficentInternal += Hooks.Ambient;
                    ShouldRun = true;
                }
            };
            Run.onRunDestroyGlobal += (Run run) =>
            {
                CharacterBody.onBodyAwakeGlobal -= Body.BodyChanges;
                RecalculateStatsAPI.GetStatCoefficients -= Hooks.RecalculateStatsAPI_GetStatCoefficients;
                CharacterMaster.onStartGlobal -= Master.MasterChanges;
                IL.EntityStates.AI.Walker.LookBusy.OnEnter -= Hooks.LookBusy_OnEnter;
                IL.EntityStates.AI.Walker.LookBusy.PickNewTargetLookDirection -= Hooks.LookBusy_PickNewTargetLookDirection;
                IL.EntityStates.AI.Walker.Wander.PickNewTargetLookPosition -= Hooks.Wander_PickNewTargetLookPosition;
                HookBehavior.UndoHooks();
                IL.RoR2.Run.RecalculateDifficultyCoefficentInternal -= Hooks.Ambient;
                ShouldRun = false;
            };
        }

        private void RandomizeConfig()
        {
            RandomizeScaling();
            RandomizeLevelAttackSpeed();
            RandomizeLevelMoveSpeed();
            RandomizeLevelRegen();
            RandomizeLoopArmor();
            RandomizeBossHp();
            RandomizeProjectileSpeed();
            RandomizeLevelDiffBoost();
            RandomizeCreditMultiplier();
            RandomizeAIScaling();
            RandomizeMonsterLimit();
            RandomizeTeleporterSize();
            RandomizeTeleporterSpeed();
            RandomizeCooldownScaling();
        }

        #region FuckOff

        private void RandomizeScaling()
        {
            Scaling.Value = (int)Math.Round((float)rnggggggggggggg.Next(0, 64), MidpointRounding.ToEven) * 25;
        }

        private void RandomizeLevelRegen()
        {
            LevelRegen.Value = (float)Math.Round(random.NextDouble(), 2);
        }

        private void RandomizeLevelMoveSpeed()
        {
            LevelMoveSpeed.Value = (float)Math.Round(random.NextDouble(), 2);
        }

        private void RandomizeLevelAttackSpeed()
        {
            LevelAttackSpeed.Value = (float)Math.Round(randomAttackSpeedFuckYou.NextDouble() * (0.1f - 0f) + 0f, 3);
        }

        private void RandomizeLoopArmor()
        {
            LoopArmor.Value = (float)Math.Round((float)rnggggggggggggg.Next(1, 10), MidpointRounding.ToEven) * 5f;
        }

        private void RandomizeProjectileSpeed()
        {
            ProjectileSpeed.Value = (float)Math.Round((float)rnggggggggggggg.Next(20, 100), MidpointRounding.ToEven) * 0.05f;
        }

        private void RandomizeLevelDiffBoost()
        {
            LevelDiffBoost.Value = (float)Math.Round((float)rnggggggggggggg.Next(0, 40), MidpointRounding.ToEven) * 0.25f;
        }

        private void RandomizeMonsterLimit()
        {
            MonsterLimit.Value = random.Next(0, 200);
        }

        private void RandomizeBossHp()
        {
            BossHp.Value = (float)Math.Round((float)rnggggggggggggg.Next(0, 50), MidpointRounding.ToEven) * 0.01f;
        }

        private void RandomizeCreditMultiplier()
        {
            CreditMultiplier.Value = (float)Math.Round((float)rnggggggggggggg.Next(0, 25), MidpointRounding.ToEven) * 0.01f;
        }

        private void RandomizeTeleporterSize()
        {
            TeleporterSize.Value = (float)Math.Round((float)rnggggggggggggg.Next(-10, 10), MidpointRounding.ToEven) * 5f;
        }

        private void RandomizeTeleporterSpeed()
        {
            TeleporterSpeed.Value = (float)Math.Round((float)rnggggggggggggg.Next(-10, 10), MidpointRounding.ToEven) * 5f;
        }

        private void RandomizeAIScaling()
        {
            AIScaling.Value = (float)Math.Round((float)rnggggggggggggg.Next(0, 30), MidpointRounding.ToEven) * 0.1f;
        }

        private void RandomizeCooldownScaling()
        {
            StageCooldownReduction.Value = (float)Math.Round((float)rnggggggggggggg.Next(0, 100), MidpointRounding.ToEven) * 0.01f;
        }

        #endregion

        private void ResetConfig()
        {
            Scaling.Value = 150f;
            LevelRegen.Value = 0.12f;
            LevelMoveSpeed.Value = 0.12f;
            LevelAttackSpeed.Value = 0.003f;
            LoopArmor.Value = 10f;
            MonsterHp.Value = 0.06f;
            BossHp.Value = 0.07f;
            StageCooldownReduction.Value = 0.01f;
            ProjectileSpeed.Value = 1.25f;
            LevelDiffBoost.Value = 1.25f;
            LevelDiffBoostScaleWithPlayers.Value = false;
            AllyPermanentDamage.Value = 0f;
            MonsterLimit.Value = 60;
            EnableAI.Value = true;
            AIScaling.Value = 1f;
            EnableSkills.Value = true;
            EnableStats.Value = true;
            EnableCDirector.Value = true;
            CreditMultiplier.Value = 0.05f;
            TeleporterSpeed.Value = 30f;
            TeleporterSize.Value = -30f;
            MithrixAS.Value = false;
        }

        private void ResetPostProcessing()
        {
            ColorGradingRedGain.Value = 3.7f;
            VignetteIntensity.Value = 0.21f;
            PostProcessingWeight.Value = 1f;
        }

        public void FixedUpdate()
        {
            if (ShouldRun)
            {
                ppHolder.SetActive(true);
                cg.enabled.value = true;
                cg.contrast.value = 70f;
                cg.gain.value = new Vector4(ColorGradingRedGain.Value, 1f, 1f, 2.5f);
                vn.intensity.value = VignetteIntensity.Value;
                cg.hueShift.value = 0f;
                cg.postExposure.value = 0f;
                cg.saturation.value = -9.610426f;
                cg.temperature.value = 0f;
                cg.tint.value = 0f;
                vn.mode.value = VignetteMode.Classic;
                vn.color.value = new Color32(105, 30, 37, 255);
                vn.smoothness.value = 0.7f;
                vn.rounded.value = false;
                ppHolder.GetComponent<PostProcessVolume>().weight = PostProcessingWeight.Value;
            }
            else
            {
                ppHolder.SetActive(false);
            }
            if (InfernoSkinMod.InfernoSkinModPlugin.BanditSkinn != null)
            {
                InfernoSkinMod.InfernoSkinModPlugin.BanditSkinn.unlockableDef = BanditSkin;
            }
            if (InfernoSkinMod.InfernoSkinModPlugin.CaptainSkinn != null)
            {
                InfernoSkinMod.InfernoSkinModPlugin.CaptainSkinn.unlockableDef = CaptainSkin;
            }
            if (InfernoSkinMod.InfernoSkinModPlugin.CommandoSkinn != null)
            {
                InfernoSkinMod.InfernoSkinModPlugin.CommandoSkinn.unlockableDef = CommandoSkin;
            }
        }

        public void ChangeAmbientCap(Run run, RuleBook useless)
        {
            Run.ambientLevelCap = (run.selectedDifficulty == InfernoDiffIndex) ? int.MaxValue : 99;
        }

        private static string Language_GetLocalizedStringByToken(On.RoR2.Language.orig_GetLocalizedStringByToken orig, Language self, string token)
        {
            InfernoDiffDef.scalingValue = Scaling.Value / 50f;
            if (token == "INFERNO_DESCRIPTION")
            {
                return "For veteran players. Every step requires utmost focus and awareness. You will be obliterated.<style=cStack>\n\n>Player Health Regeneration: <style=cIsHealth>-40%</style> \n" +
                                                   (Scaling.Value <= 0f ? ">Difficulty Scaling: <style=cIsHealth>0%</style>\n" : (Scaling.Value == 100f ? "" : (Scaling.Value < 100f ? ">Difficulty Scaling: <style=cIsHealing>" + (Scaling.Value - 100f) + "% + Endless</style>\n" : ">Difficulty Scaling: <style=cIsHealth>+" + (Scaling.Value - 100f) + "% + Endless</style>\n"))) +
                                                   ((LevelAttackSpeed.Value > 0f || LevelMoveSpeed.Value > 0f || LevelRegen.Value > 0f) ? ">Enemy Stats: <style=cIsHealth>Constantly Increasing</style>\n" : "") +
                                                   (ProjectileSpeed.Value > 1f ? ">Enemy Projectile Speed: <style=cIsHealth>+" + ((ProjectileSpeed.Value - 1f) * 100f) + "%</style>\n" : "") +
                                                   (EnableCDirector.Value ? ">Combat Director: <style=cIsHealth>Resourceful</style>\n" : "") +
                                                   (LevelDiffBoost.Value > 0f ? ">Starting Difficulty: <style=cIsHealth>Increased</style>\n" : "") +
                                                   (EnableSkills.Value || EnableStats.Value ? ">Enemy Abilities: <style=cIsHealth>Improved</style>\n" : "") +
                                                   (EnableAI.Value ? ">Enemy AI: <style=cIsHealth>Refined" + (AIScaling.Value > 0f ? " + Evolving</style>\n" : "</style>\n") : "") +
                                                   (MonsterLimit.Value != 40f ? (MonsterLimit.Value < 40f ? ">Enemy Cap: <style=cIsHealing>" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>\n" : ">Enemy Cap: <style=cIsHealth>+" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>\n") : "") +
                                                   (AllyPermanentDamage.Value > 0f ? ">Allies receive <style=cIsHealth>permanent damage</style>\n" : "") +
                                                   "</style>";
            }
            return orig(self, token);
        }

        public void AddDifficulty()
        {
            InfernoDiffDef = new((Scaling.Value / 50f), "INFERNO_NAME", "INFERNO_ICON", "INFERNO_DESCRIPTION", new Color32(105, 30, 37, 255), "if", true);
            InfernoDiffDef.iconSprite = inferno.LoadAsset<Sprite>("texInfernoIcon.png");
            InfernoDiffDef.foundIconSprite = true;
            InfernoDiffIndex = DifficultyAPI.AddDifficulty(InfernoDiffDef);
        }

        public void FillTokens()
        {
            InfernoDiffDef.scalingValue = Scaling.Value / 50f;
            LanguageAPI.Add("INFERNO_NAME", "Inferno");
            LanguageAPI.Add("INFERNO_DESCRIPTION", "For veteran players. Every step requires utmost focus and awareness. You will be obliterated.<style=cStack>\n\n>Player Health Regeneration: <style=cIsHealth>-40%</style> \n" +
                                                   (Scaling.Value <= 0f ? ">Difficulty Scaling: <style=cIsHealth>0%</style>\n" : (Scaling.Value == 100f ? "" : (Scaling.Value < 100f ? ">Difficulty Scaling: <style=cIsHealing>" + (Scaling.Value - 100f) + "% + Endless</style>\n" : ">Difficulty Scaling: <style=cIsHealth>+" + (Scaling.Value - 100f) + "% + Endless</style>\n"))) +
                                                   ((LevelAttackSpeed.Value > 0f || LevelMoveSpeed.Value > 0f || LevelRegen.Value > 0f) ? ">Enemy Stats: <style=cIsHealth>Constantly Increasing</style>\n" : "") +
                                                   (ProjectileSpeed.Value > 1f ? ">Enemy Projectile Speed: <style=cIsHealth>+" + ((ProjectileSpeed.Value - 1f) * 100f) + "%</style>\n" : "") +
                                                   (EnableCDirector.Value ? ">Combat Director: <style=cIsHealth>Resourceful</style>\n" : "") +
                                                   (LevelDiffBoost.Value > 0f ? ">Starting Difficulty: <style=cIsHealth>Increased</style>\n" : "") +
                                                   (EnableSkills.Value || EnableStats.Value ? ">Enemy Abilities: <style=cIsHealth>Improved</style>\n" : "") +
                                                   (EnableAI.Value ? ">Enemy AI: <style=cIsHealth>Refined" + (AIScaling.Value > 0f ? " + Evolving</style>\n" : "</style>\n") : "") +
                                                   (MonsterLimit.Value != 40f ? (MonsterLimit.Value < 40f ? ">Enemy Cap: <style=cIsHealing>" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>" : ">Enemy Cap: <style=cIsHealth>+" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>\n") : "") +
                                                   (AllyPermanentDamage.Value > 0f ? ">Allies receive <style=cIsHealth>permanent damage</style>\n" : "") +
                                                   "</style>");

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOCLEARGAMEINFERNO_NAME", "Commando: Survival");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOCLEARGAMEINFERNO_DESCRIPTION", "As Commando, beat the game or obliterate on Inferno.");

            LanguageAPI.Add("ACHIEVEMENT_BANDIT2CLEARGAMEINFERNO_NAME", "Bandit: Survival");
            LanguageAPI.Add("ACHIEVEMENT_BANDIT2CLEARGAMEINFERNO_DESCRIPTION", "As Bandit, beat the game or obliterate on Inferno.");

            LanguageAPI.Add("ACHIEVEMENT_CAPTAINCLEARGAMEINFERNO_NAME", "Captain: Survival");
            LanguageAPI.Add("ACHIEVEMENT_CAPTAINCLEARGAMEINFERNO_DESCRIPTION", "As Captain, beat the game or obliterate on Inferno.");
            LanguageAPI.Add("DOTFLARE_SKIN_BCAPTAIN_NAME", "Erised");

            CommandoSkin = ScriptableObject.CreateInstance<UnlockableDef>();
            CommandoSkin.cachedName = "Skins.Inferno_Commando";
            CommandoSkin.nameToken = "ACHIEVEMENT_COMMANDOCLEARGAMEINFERNO_NAME";
            CommandoSkin.achievementIcon = inferno.LoadAsset<Sprite>("Assets/InfernoSkins/texOvergrownCommandoIcon.png");
            ContentAddition.AddUnlockableDef(CommandoSkin);

            BanditSkin = ScriptableObject.CreateInstance<UnlockableDef>();
            BanditSkin.cachedName = "Skins.Inferno_Bandit";
            BanditSkin.nameToken = "ACHIEVEMENT_BANDIT2CLEARGAMEINFERNO_NAME";
            BanditSkin.achievementIcon = inferno.LoadAsset<Sprite>("Assets/InfernoSkins/texDeadshotBanditIcon.png");
            ContentAddition.AddUnlockableDef(BanditSkin);

            CaptainSkin = ScriptableObject.CreateInstance<UnlockableDef>();
            CaptainSkin.cachedName = "Skins.Inferno_Captain";
            CaptainSkin.nameToken = "ACHIEVEMENT_CAPTAINCLEARGAMEINFERNO_NAME";
            CaptainSkin.achievementIcon = inferno.LoadAsset<Sprite>("Assets/InfernoSkins/texErisedCaptainIcon.png");
            ContentAddition.AddUnlockableDef(CaptainSkin);
        }
    }

    public class InfernoPermanentDamage : MonoBehaviour, IOnTakeDamageServerReceiver
    {
        public HealthComponent hc;
        public CharacterBody body;

        public void Start()
        {
            hc = GetComponent<HealthComponent>();
            if (!hc)
            {
                Object.Destroy(this);
                return;
            }
            body = hc.body;
        }

        public void OnTakeDamageServer(DamageReport damageReport)
        {
            if (body)
            {
                switch (body.teamComponent.teamIndex)
                {
                    case TeamIndex.Player:
                        {
                            float takenDamagePercent = damageReport.damageDealt / hc.fullCombinedHealth * 100f;
                            int permanentDamage = Mathf.FloorToInt(takenDamagePercent * Main.AllyPermanentDamage.Value / 100f);
                            for (int l = 0; l < permanentDamage; l++)
                            {
                                body.AddBuff(RoR2Content.Buffs.PermanentCurse);
                            }
                        }
                        break;
                }
            }
        }
    }

    public static class RiskyModCompat
    {
        private static bool? _enabled;

        public static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod");
                }
                return (bool)_enabled;
            }
        }
    }
}