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

namespace Inferno
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.RiskyLives.RiskyMod", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [R2APISubmoduleDependency(nameof(DifficultyAPI), nameof(LanguageAPI))]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "Inferno";
        public const string PluginVersion = "1.4.0";

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
        public static ConfigEntry<float> LevelDiffBoost { get; set; }
        public static ConfigEntry<bool> LevelDiffBoostScaleWithPlayers { get; set; }
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

        private static bool hasFired;

        public static UnlockableDef CommandoSkin;
        public static UnlockableDef BanditSkin;
        public static UnlockableDef CaptainSkin;

        private static readonly System.Random random = new();

        private static readonly System.Random rnggggggggggggg = new();

        private static readonly System.Random randomAttackSpeedFuckYou = new();

        public void Awake()
        {
            Main.InfernoLogger = base.Logger;

            inferno = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("Inferno.dll", "inferno"));
            Important = Config.Bind("General", "! Important !", true, "Make sure everyone's configs are the same for multiplayer!");
            Scaling = Config.Bind("General", "Difficulty Scaling", 150f, "Percentage of difficulty scaling, 150% is +50%. Difficulty order does not update visually. Vanilla is 150");
            LevelMoveSpeed = Config.Bind("General", "Enemy Move Speed Scaling", 0.12f, "Adds move speed to each monster every level. Vanilla is 0");
            LevelRegen = Config.Bind("General", "Enemy Regen Scaling", 0.08f, "Adds health regen to each monster every level. Vanilla is 0");
            LevelAttackSpeed = Config.Bind("General", "Enemy Attack Speed Scaling", 0.003f, "Adds attack speed to each monster every level. Vanilla is 0");
            LoopArmor = Config.Bind("General", "Enemy Armor Scaling", 10f, "Adds armor to each monster every completed loop. Vanilla is 0");
            MonsterHp = Config.Bind("General", "Enemy Health Scaling", 0.06f, "Adds %max hp to each monster every completed stage. Vanilla is 0");
            BossHp = Config.Bind("General", "Boss Health Scaling", 0.07f, "Adds %max hp to each boss every completed stage. Vanilla is 0");
            ProjectileSpeed = Config.Bind("General", "Enemy Projectile Speed", 1.25f, "Sets the projectile speed multiplier. Vanilla is 1");
            LevelDiffBoost = Config.Bind("General II", "Level and Difficulty Boost", 1.25f, "Adds to ambient level and difficulty coefficient. Vanilla is 0");
            LevelDiffBoostScaleWithPlayers = Config.Bind("General II", "Scale Difficulty Boost with player count?", false, "Vanilla is false");
            MonsterLimit = Config.Bind("General II", "Enemy Cap", 60, "Sets the enemy cap. Vanilla is 40");
            EnableAI = Config.Bind("General II", "Enable AI Changes?", true, "Vanilla is false");
            AIScaling = Config.Bind("General II", "AI Scaling Coefficient", 1.0f, "Adds to AI aim and range every cleared stage. Only works with AI Changes enabled. Vanilla is false");
            EnableSkills = Config.Bind("General II", "Enable Skill Changes?", true, "Vanilla is false");
            EnableStats = Config.Bind("General II", "Enable Stat Changes?", true, "Vanilla is false");
            EnableCDirector = Config.Bind("General II", "Enable Combat Director Changes?", true, "Makes the combat director spawn monsters closer and with more variety during looping. Vanilla is false");
            CreditMultiplier = Config.Bind("General II", "C. Director Credit Multiplier", 0.04f, "Adds to the combat director credit multiplier every cleared stage. Vanilla is 0");
            TeleporterSpeed = Config.Bind("General III", "Holdout Zone Speed", 30f, "Adds to the percent of holdout zone speed. Vanilla is 0");
            TeleporterSize = Config.Bind("General III", "Holdout Zone Radius", -30f, "Adds to the percent of holdout zone radius. Vanilla is 0");
            MithrixAS = Config.Bind("Bullshit", "Make Mithrix scale with Attack Speed?", false, "Vanilla is false");

            ColorGradingRedGain = Config.Bind("Post Processing", "Red Gain", 3.7f, "Vanilla is 0");
            VignetteIntensity = Config.Bind("Post Processing", "Vignette Intensity", 0.21f, "Vanilla is 0");
            PostProcessingWeight = Config.Bind("Post Processing", "Strength", 1f, "Vanilla is 0");

            #region WhoCares

            AddDifficulty();

            FillTokens();

            ppHolder = new("PPInferno");
            DontDestroyOnLoad(ppHolder);
            ppHolder.layer = RoR2.LayerIndex.postProcess.intVal;
            ppHolder.AddComponent<InfernoPostProcessingController>();
            PostProcessVolume pp = ppHolder.AddComponent<PostProcessVolume>();
            DontDestroyOnLoad(pp);
            pp.isGlobal = true;
            pp.weight = 1f;
            pp.priority = 49;
            PostProcessProfile ppProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
            DontDestroyOnLoad(ppProfile);
            ppProfile.name = "ppInferno";
            cg = ppProfile.AddSettings<ColorGrading>();
            cg.SetAllOverridesTo(true);
            vn = ppProfile.AddSettings<Vignette>();
            vn.SetAllOverridesTo(true);

            pp.sharedProfile = ppProfile;

            #endregion WhoCares

            ModSettingsManager.SetModIcon(inferno.LoadAsset<Sprite>("texInferno2Icon.png"));
            ModSettingsManager.AddOption(new GenericButtonOption("! Important ! ", "General", "Make sure everyone's configs are the same for multiplayer!", "                                      ", ResetConfig));
            ModSettingsManager.AddOption(new StepSliderOption(Scaling, new StepSliderConfig() { increment = 25f, min = 0f, max = 1600f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelAttackSpeed, new StepSliderConfig() { increment = 0.001f, min = 0f, max = 0.1f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelMoveSpeed, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
            ModSettingsManager.AddOption(new StepSliderOption(LevelRegen, new StepSliderConfig() { increment = 0.01f, min = 0f, max = 1f }));
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
                        CharacterBody.onBodyAwakeGlobal += BodyChanges;
                        RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
                    }
                    if (EnableAI.Value)
                    {
                        CharacterMaster.onStartGlobal += MasterChanges;
                        IL.EntityStates.AI.Walker.LookBusy.OnEnter += LookBusy_OnEnter;
                        IL.EntityStates.AI.Walker.LookBusy.PickNewTargetLookDirection += LookBusy_PickNewTargetLookDirection;
                        IL.EntityStates.AI.Walker.Wander.PickNewTargetLookPosition += Wander_PickNewTargetLookPosition;
                    }
                    if (EnableSkills.Value)
                    {
                        ApplyHooks();
                    }
                    IL.RoR2.Run.RecalculateDifficultyCoefficentInternal += Ambient;
                    ShouldRun = true;
                }
            };
            Run.onRunDestroyGlobal += (Run run) =>
            {
                CharacterBody.onBodyAwakeGlobal -= BodyChanges;
                RecalculateStatsAPI.GetStatCoefficients -= RecalculateStatsAPI_GetStatCoefficients;
                CharacterMaster.onStartGlobal -= MasterChanges;
                IL.EntityStates.AI.Walker.LookBusy.OnEnter -= LookBusy_OnEnter;
                IL.EntityStates.AI.Walker.LookBusy.PickNewTargetLookDirection -= LookBusy_PickNewTargetLookDirection;
                IL.EntityStates.AI.Walker.Wander.PickNewTargetLookPosition -= Wander_PickNewTargetLookPosition;
                UndoHooks();
                IL.RoR2.Run.RecalculateDifficultyCoefficentInternal -= Ambient;
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

        #endregion FuckOff

        private void ResetConfig()
        {
            Scaling.Value = 150f;
            LevelRegen.Value = 0.08f;
            LevelMoveSpeed.Value = 0.12f;
            LevelAttackSpeed.Value = 0.003f;
            LoopArmor.Value = 10f;
            ProjectileSpeed.Value = 1.25f;
            LevelDiffBoost.Value = 1.25f;
            MonsterLimit.Value = 60;
            EnableAI.Value = true;
            EnableSkills.Value = true;
            EnableStats.Value = true;
            EnableCDirector.Value = true;
            LevelDiffBoostScaleWithPlayers.Value = false;
            MithrixAS.Value = false;
            BossHp.Value = 0.05f;
            CreditMultiplier.Value = 0.05f;
            TeleporterSize.Value = -30f;
            TeleporterSpeed.Value = 30f;
            AIScaling.Value = 1f;
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
                                                   (MonsterLimit.Value != 40f ? (MonsterLimit.Value < 40f ? ">Enemy Cap: <style=cIsHealing>" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>" : ">Enemy Cap: <style=cIsHealth>+" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>") : "") +
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
                                                   (MonsterLimit.Value != 40f ? (MonsterLimit.Value < 40f ? ">Enemy Cap: <style=cIsHealing>" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>" : ">Enemy Cap: <style=cIsHealth>+" + ((((float)MonsterLimit.Value - 40f) / 40f) * 100f) + "%</style>") : "") +
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

        public void BodyChanges(CharacterBody body)
        {
            var cb = body.GetComponent<CharacterBody>();
            var ssoh = body.GetComponent<SetStateOnHurt>();
            var cd = body.GetComponent<CharacterDirection>();
            switch (cd == null)
            {
                case false:
                    cd.turnSpeed = 360f;
                    break;

                default:
                    break;
            }

            switch (ssoh == null)
            {
                case false:
                    ssoh.canBeHitStunned = false;
                    break;

                default:
                    break;
            }

            switch (body.name)
            {
                default:
                    break;

                case "BeetleQueen2Body(Clone)":
                    cb.baseMaxHealth = 2500f;
                    cb.levelMaxHealth = 750f;
                    cb.mainRootSpeed = 14f;
                    cb.baseMoveSpeed = 14f;
                    cb.rootMotionInMainState = false;
                    cd.driveFromRootRotation = false;
                    cd.turnSpeed = 100f;
                    break;

                case "ClayBossBody(Clone)":
                    cb.baseMaxHealth = 2500f;
                    cb.levelMaxHealth = 750f;
                    cb.baseMoveSpeed = 14f;
                    break;

                case "GrandParentBody(Clone)":
                    cb.baseMaxHealth = 4000f;
                    cb.levelMaxHealth = 1200f;
                    break;

                case "GravekeeperBody(Clone)":
                    cb.baseMaxHealth = 3200f;
                    cb.levelMaxHealth = 960f;
                    cb.baseMoveSpeed = 24f;
                    break;

                case "GravekeeperTrackingFireball(Clone)":
                    cb.baseMoveSpeed = 50f;
                    cb.baseMaxHealth = 50f;
                    cb.levelMaxHealth = 15f;
                    break;

                case "ImpBossBody(Clone)":
                    cb.baseMaxHealth = 3200f;
                    cb.levelMaxHealth = 960f;
                    cb.baseMoveSpeed = 13f;
                    cb.baseAcceleration = 200f;
                    break;

                case "MagmaWormBody(Clone)":
                    cb.baseMaxHealth = 2500f;
                    cb.levelMaxHealth = 750f;
                    body.GetComponent<WormBodyPositions2>().speedMultiplier = 40f;
                    body.GetComponent<WormBodyPositions2>().followDelay = 0.1f;
                    body.GetComponent<WormBodyPositionsDriver>().maxTurnSpeed = 1000f;
                    break;

                case "ElectricWormBody(Clone)":
                    body.GetComponent<WormBodyPositions2>().speedMultiplier = 40f;
                    body.GetComponent<WormBodyPositions2>().followDelay = 0.1f;
                    body.GetComponent<WormBodyPositionsDriver>().maxTurnSpeed = 1000f;
                    break;

                case "BrotherBody(Clone)":
                    cb.baseAcceleration = 200f;
                    cb.baseMoveSpeed = 17f;
                    cb.baseMaxHealth = 800f;
                    cb.levelMaxHealth = 240f;
                    cb.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                    cb.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
                    break;

                case "BrotherHurtBody(Clone)":
                    cb.baseMaxHealth = 500f;
                    cb.levelMaxHealth = 150f;
                    cb.baseMoveSpeed = 10f;
                    cb.sprintingSpeedMultiplier = 1.45f;
                    cb.baseDamage = 5f;
                    cb.levelDamage = 1f;
                    break;

                case "TitanBody(Clone)":
                    cb.baseMaxHealth = 2500f;
                    cb.levelMaxHealth = 750f;
                    cb.baseMoveSpeed = 12f;
                    break;

                case "VagrantBody(Clone)":
                    cb.baseMaxHealth = 2500f;
                    cb.levelMaxHealth = 750f;
                    cb.baseMoveSpeed = 14f;
                    cb.baseAcceleration = 500f;
                    break;

                case "BeetleBody(Clone)":
                    cb.baseMoveSpeed = 12f;
                    ssoh.canBeStunned = false;
                    cb.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                    break;

                case "BeetleGuardBody(Clone)":
                    cb.baseMoveSpeed = 22f;
                    break;

                case "BisonBody(Clone)":
                    cb.baseMoveSpeed = 5f;
                    cb.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                    break;

                case "FlyingVerminBody(Clone)":
                    cb.baseMoveSpeed = 9f;
                    cb.baseDamage = 10f;
                    cb.levelDamage = 2f;
                    break;

                case "ClayBruiserBody(Clone)":
                    cb.baseMoveSpeed = 11f;
                    cb.baseDamage = 13f;
                    cb.levelDamage = 2.6f;
                    break;

                case "LemurianBruiserBody(Clone)":
                    cb.baseMoveSpeed = 16f;
                    break;

                case "GreaterWispBody(Clone)":
                    cb.baseMoveSpeed = 12f;
                    break;

                case "HermitCrabBody(Clone)":
                    cb.baseMoveSpeed = 22f;
                    break;

                case "ImpBody(Clone)":
                    cb.baseMoveSpeed = 15f;
                    break;

                case "JellyfishBody(Clone)":
                    cb.baseMoveSpeed = 13f;
                    break;

                case "LemurianBody(Clone)":
                    cb.baseMoveSpeed = 10f;
                    break;

                case "WispBody(Clone)":
                    cb.baseMoveSpeed = 12f;
                    cb.baseAcceleration = 24f;
                    ssoh.canBeStunned = false;
                    break;

                case "LunarExploderBody(Clone)":
                    cb.baseMoveSpeed = 16f;
                    cb.baseAcceleration = 1000f;
                    break;

                case "LunarGolemBody(Clone)":
                    cb.baseMoveSpeed = 16f;
                    break;

                case "LunarWispBody(Clone)":
                    cb.baseDamage = 9f;
                    break;

                case "GolemBody(Clone)":
                    cb.baseDamage = 16f;
                    cb.levelDamage = 3.2f;
                    cb.baseMoveSpeed = 8f;
                    break;

                case "NullifierBody(Clone)":
                    cb.baseMoveSpeed = 17f;
                    ssoh.canBeStunned = false;
                    break;

                case "MegaConstructBody(Clone)":
                    cb.baseMoveSpeed = 35f;
                    break;

                case "GupBody(Clone)":
                    cb.baseMoveSpeed = 14f;
                    cb.baseMaxHealth = 700f;
                    cb.levelMaxHealth = 210f;
                    break;

                case "GeepBody(Clone)":
                    cb.baseMoveSpeed = 19f;
                    cb.baseMaxHealth = 550f;
                    cb.levelMaxHealth = 165f;
                    cb.baseDamage = 11f;
                    cb.levelDamage = 2.2f;
                    break;

                case "GipBody(Clone)":
                    cb.baseMoveSpeed = 24f;
                    cb.baseMaxHealth = 350f;
                    cb.levelMaxHealth = 105f;
                    cb.baseDamage = 10f;
                    cb.levelDamage = 2f;
                    break;

                case "ClayGrenadierBody(Clone)":
                    cb.baseMoveSpeed = 12f;
                    break;

                case "ParentBody(Clone)":
                    cb.baseMoveSpeed = 17f;
                    break;

                case "MiniMushroomBody(Clone)":
                    cb.baseMoveSpeed = 6f;
                    break;

                case "VagrantTrackingBomb(Clone)":
                    cb.baseMaxHealth = 90f;
                    cb.levelMaxHealth = 27f;
                    break;

                case "RoboBallBossBody(Clone)":
                    cb.baseMoveSpeed = 14f;
                    cb.baseMaxHealth = 3000f;
                    cb.levelMaxHealth = 900f;
                    break;

                case "ScavBody(Clone)":
                    cb.baseMoveSpeed = 9f;
                    break;

                case "AcidLarvaBody(Clone)":
                    cb.sprintingSpeedMultiplier = 3f;
                    cb.baseMaxHealth = 70f;
                    cb.levelMaxHealth = 21f;
                    break;

                case "TitanGoldBody(Clone)":
                    cb.baseMaxHealth = 2500f;
                    cb.levelMaxHealth = 750f;
                    cb.baseMoveSpeed = 12f;
                    break;

                case "RoboBallMiniBody(Clone)":
                    cb.baseMoveSpeed = 13f;
                    break;
            }
        }

        public void MasterChanges(CharacterMaster master)
        {
            switch (master.teamIndex)
            {
                default:
                    break;

                case TeamIndex.Monster:

                    switch (master.bodyInstanceObject.GetComponent<CharacterBody>() == null)
                    {
                        case false:
                            var cb = master.bodyInstanceObject.GetComponent<CharacterBody>();
                            cb.levelMoveSpeed = LevelMoveSpeed.Value;
                            cb.levelRegen = LevelRegen.Value;
                            cb.levelAttackSpeed = LevelAttackSpeed.Value;
                            if (cb.baseJumpCount < 2)
                            {
                                cb.baseJumpPower = 2;
                            }
                            if (cb.baseJumpPower < 20f)
                            {
                                cb.baseJumpPower = 20f;
                            }
                            break;

                        default:
                            break;
                    }
                    switch (master.GetComponent<BaseAI>() != null && master.name != "GolemMaster(Clone)")
                    {
                        default:
                            break;

                        case true:
                            var ba = master.GetComponent<BaseAI>();
                            ba.fullVision = true;
                            ba.aimVectorDampTime = Mathf.Max(0.001f, 0.031f - (0.0001f * AIScaling.Value * Run.instance.stageClearCount));
                            ba.aimVectorMaxSpeed = 250f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                            ba.enemyAttentionDuration = 1.5f;
                            break;
                    }

                    switch (master.GetComponent<BaseAI>() != null && master.name == "GolemMaster(Clone)")
                    {
                        default:
                            break;

                        case true:
                            var ba = master.GetComponent<BaseAI>();
                            ba.fullVision = true;
                            ba.aimVectorDampTime = Mathf.Max(0.001f, 0.09f - (0.0003f * AIScaling.Value * Run.instance.stageClearCount));
                            ba.aimVectorMaxSpeed = 250f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                            ba.enemyAttentionDuration = 1.5f;
                            break;
                    }
                    break;
            }

            var masterm = master.GetComponent<CharacterMaster>();

            switch (master.name)
            {
                default:
                    break;

                case "BeetleQueenMaster(Clone)":
                    AISkillDriver BeetleQueenChase2 = (from x in master.GetComponents<AISkillDriver>()
                                                       where x.customName == "Chase"
                                                       select x).First();
                    BeetleQueenChase2.minDistance = 0f;
                    AISkillDriver BeetleQueenFireFuckwards = (from x in master.GetComponents<AISkillDriver>()
                                                              where x.customName == "SpawnWards"
                                                              select x).First();
                    BeetleQueenFireFuckwards.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    BeetleQueenFireFuckwards.maxUserHealthFraction = Mathf.Infinity;
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "GravekeeperMaster(Clone)":

                    AISkillDriver GrovetenderRunAndShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                            where x.customName == "RunAndShoot"
                                                            select x).First();
                    GrovetenderRunAndShoot.movementType = AISkillDriver.MovementType.StrafeMovetarget;

                    AISkillDriver GrovetenderHook = (from x in masterm.GetComponents<AISkillDriver>()
                                                     where x.customName == "Hooks"
                                                     select x).First();
                    GrovetenderHook.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    GrovetenderHook.maxUserHealthFraction = Mathf.Infinity;
                    GrovetenderHook.minDistance = 13f;

                    AISkillDriver GrovetenderFuckAround = (from x in masterm.GetComponents<AISkillDriver>()
                                                           where x.customName == "WaitAroundUntilSkillIsBack"
                                                           select x).First();
                    GrovetenderFuckAround.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    break;

                case "ImpBossMaster(Clone)":
                    AISkillDriver ImpOverlordGroundPound = (from x in masterm.GetComponents<AISkillDriver>()
                                                            where x.customName == "GroundPound"
                                                            select x).First();
                    ImpOverlordGroundPound.maxDistance = 15f;

                    AISkillDriver ImpOverlordSpike = (from x in masterm.GetComponents<AISkillDriver>()
                                                      where x.customName == "FireVoidspikesWhenInRange"
                                                      select x).First();
                    ImpOverlordSpike.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    ImpOverlordSpike.minDistance = 16f;
                    AISkillDriver ImpOverlordTeleport = (from x in masterm.GetComponents<AISkillDriver>()
                                                         where x.customName == "BlinkToTarget"
                                                         select x).First();
                    ImpOverlordTeleport.minDistance = 33f;
                    break;

                case "BrotherMaster(Clone)":
                    AISkillDriver MithrixFireShards = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.customName == "Sprint and FireLunarShards"
                                                       select x).First();
                    MithrixFireShards.minDistance = 0f;
                    MithrixFireShards.maxUserHealthFraction = Mathf.Infinity;
                    AISkillDriver MithrixSprint = (from x in masterm.GetComponents<AISkillDriver>()
                                                   where x.customName == "Sprint After Target"
                                                   select x).First();
                    MithrixSprint.minDistance = 40f - (AIScaling.Value * Run.instance.stageClearCount);
                    break;

                case "BrotherHurtMaster(Clone)":
                    AISkillDriver MithrixWeakSlam = (from x in masterm.GetComponents<AISkillDriver>()
                                                     where x.customName == "SlamGround"
                                                     select x).First();
                    MithrixWeakSlam.maxUserHealthFraction = Mathf.Infinity;
                    MithrixWeakSlam.movementType = AISkillDriver.MovementType.StrafeMovetarget;

                    AISkillDriver MithrixWeakShards = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.customName == "Shoot"
                                                       select x).First();
                    MithrixWeakShards.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    // CreateDrivers(master);
                    break;

                case "TitanMaster(Clone)":
                    AISkillDriver StoneTitanLaser = (from x in masterm.GetComponents<AISkillDriver>()
                                                     where x.skillSlot == SkillSlot.Special
                                                     select x).First();
                    StoneTitanLaser.maxUserHealthFraction = Mathf.Infinity;
                    StoneTitanLaser.minDistance = 16f;
                    StoneTitanLaser.movementType = AISkillDriver.MovementType.StrafeMovetarget;

                    AISkillDriver StoneTitanRockTurret = (from x in masterm.GetComponents<AISkillDriver>()
                                                          where x.skillSlot == SkillSlot.Utility
                                                          select x).First();
                    StoneTitanRockTurret.maxUserHealthFraction = 0.8f;
                    StoneTitanRockTurret.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    break;

                case "MinorConstructMaster(Clone)":
                    AISkillDriver AlphaConstructFire = (from x in masterm.GetComponents<AISkillDriver>()
                                                        where x.customName == "Shooty"
                                                        select x).First();
                    AlphaConstructFire.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    break;

                case "BeetleMaster(Clone)":
                    AISkillDriver BeetleHeadbutt = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "HeadbuttOffNodegraph"
                                                    select x).First();
                    BeetleHeadbutt.maxDistance = 25f;
                    BeetleHeadbutt.selectionRequiresOnGround = true;
                    BeetleHeadbutt.activationRequiresAimTargetLoS = true;
                    break;

                case "BeetleGuardMaster(Clone)":
                    AISkillDriver BeetleGuardFireSunder = (from x in masterm.GetComponents<AISkillDriver>()
                                                           where x.customName == "FireSunder"
                                                           select x).First();
                    BeetleGuardFireSunder.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine, 2);
                    break;

                case "FlyingVerminMaster(Clone)":
                    AISkillDriver BlindPestSpit = (from x in masterm.GetComponents<AISkillDriver>()
                                                   where x.skillSlot == SkillSlot.Primary
                                                   select x).First();
                    BlindPestSpit.minDistance = 10f;
                    BlindPestSpit.maxDistance = 40f;
                    break;

                case "ClayBruiserMaster(Clone)":
                    AISkillDriver ClayTemplarShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                      where x.customName == "WalkAndShoot"
                                                      select x).First();
                    ClayTemplarShoot.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    break;

                case "LemurianBruiserMaster(Clone)":
                    AISkillDriver ElderLemurianStop = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.customName == "StopAndShoot"
                                                       select x).First();
                    ElderLemurianStop.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                    break;

                case "GreaterWispMaster(Clone)":
                    AISkillDriver GreaterWispShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                      where x.minDistance == 15f
                                                      select x).First();
                    GreaterWispShoot.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    GreaterWispShoot.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    break;

                case "ImpMaster(Clone)":
                    AISkillDriver ImpSlash = (from x in masterm.GetComponents<AISkillDriver>()
                                              where x.customName == "Slash"
                                              select x).First();
                    ImpSlash.maxDistance = 12f;
                    break;

                case "LemurianMaster(Clone)":
                    AISkillDriver LemurianBite = (from x in masterm.GetComponents<AISkillDriver>()
                                                  where x.customName == "ChaseAndBiteOffNodegraph"
                                                  select x).First();
                    LemurianBite.maxDistance = 8f;

                    AISkillDriver LemurianBiteSlow = (from x in masterm.GetComponents<AISkillDriver>()
                                                      where x.customName == "ChaseAndBiteOffNodegraphWhileSlowingDown"
                                                      select x).First();
                    LemurianBiteSlow.maxDistance = 0f;

                    AISkillDriver LemurianShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                   where x.customName == "StrafeAndShoot"
                                                   select x).First();
                    LemurianShoot.minDistance = 10f;
                    LemurianShoot.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);

                    AISkillDriver LemurianStrafe = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "StrafeIdley"
                                                    select x).First();

                    LemurianStrafe.minDistance = 10f;
                    LemurianStrafe.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    break;

                case "WispMaster(Clone)":
                    AISkillDriver LesserWispSomething = (from x in masterm.GetComponents<AISkillDriver>()
                                                         where x.minDistance == 0
                                                         select x).First();
                    LesserWispSomething.maxDistance = 10f;

                    AISkillDriver LesserWispSomething2 = (from x in masterm.GetComponents<AISkillDriver>()
                                                          where x.maxDistance == 30
                                                          select x).First();
                    LesserWispSomething2.minDistance = 10f;
                    break;

                case "LunarExploderMaster(Clone)":
                    AISkillDriver LunarExploderShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                        where x.customName == "StrafeAndShoot"
                                                        select x).First();
                    LunarExploderShoot.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    AISkillDriver LunarExploderSprintShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                              where x.customName == "SprintNodegraphAndShoot"
                                                              select x).First();
                    LunarExploderSprintShoot.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "GolemMaster(Clone)":
                    AISkillDriver StoneGolemShootLaser = (from x in masterm.GetComponents<AISkillDriver>()
                                                          where x.skillSlot == SkillSlot.Secondary
                                                          select x).First();
                    StoneGolemShootLaser.selectionRequiresAimTarget = true;
                    StoneGolemShootLaser.activationRequiresAimTargetLoS = true;
                    StoneGolemShootLaser.activationRequiresAimConfirmation = true;
                    StoneGolemShootLaser.maxDistance = 100f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    StoneGolemShootLaser.minDistance = 0f;
                    break;

                case "NullifierMaster(Clone)":
                    AISkillDriver VoidReaverPanicFire = (from x in masterm.GetComponents<AISkillDriver>()
                                                         where x.customName == "PanicFireWhenClose"
                                                         select x).First();
                    VoidReaverPanicFire.movementType = AISkillDriver.MovementType.ChaseMoveTarget;

                    AISkillDriver VoidReaverTrack = (from x in masterm.GetComponents<AISkillDriver>()
                                                     where x.customName == "FireAndStrafe"
                                                     select x).First();
                    VoidReaverTrack.movementType = AISkillDriver.MovementType.ChaseMoveTarget;

                    AISkillDriver VoidReaverStop = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "FireAndChase"
                                                    select x).First();
                    VoidReaverStop.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                    break;

                case "MegaConstructMaster(Clone)":
                    AISkillDriver XiConstructLazer = (from x in masterm.GetComponents<AISkillDriver>()
                                                      where x.skillSlot == SkillSlot.Primary
                                                      select x).First();
                    XiConstructLazer.maxDistance = 50f;
                    XiConstructLazer.minDistance = 25f;
                    AISkillDriver XiConstructShield = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.skillSlot == SkillSlot.Utility
                                                       select x).First();
                    XiConstructShield.maxDistance = 50f;
                    XiConstructShield.minDistance = 25f;
                    AISkillDriver XiConstructSummon = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.skillSlot == SkillSlot.Special
                                                       select x).First();
                    XiConstructSummon.maxDistance = 50f;
                    XiConstructSummon.minDistance = 25f;
                    AISkillDriver XiConstructStrafeStep = (from x in masterm.GetComponents<AISkillDriver>()
                                                           where x.customName == "StrafeStep"
                                                           select x).First();
                    XiConstructSummon.maxDistance = 50f;
                    XiConstructSummon.minDistance = 25f;
                    break;

                case "GupMaster(Clone)":
                    AISkillDriver GupSpike = (from x in masterm.GetComponents<AISkillDriver>()
                                              where x.customName == "Spike"
                                              select x).First();
                    GupSpike.maxDistance = 14f;
                    break;

                case "ClayGrenadierMaster(Clone)":
                    AISkillDriver ClayApothecaryFaceslam = (from x in masterm.GetComponents<AISkillDriver>()
                                                            where x.customName == "FaceSlam"
                                                            select x).First();
                    ClayApothecaryFaceslam.maxDistance = 35f + (7f * AIScaling.Value * Run.instance.stageClearCount);
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "ParentMaster(Clone)":
                    AISkillDriver ParentTeleport = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "Teleport"
                                                    select x).First();
                    ParentTeleport.minDistance = 13f;
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "LunarWispMaster(Clone)":
                    AISkillDriver LunarWispBackUp = (from x in masterm.GetComponents<AISkillDriver>()
                                                     where x.customName == "Back Up"
                                                     select x).First();
                    LunarWispBackUp.maxDistance = 13f;
                    AISkillDriver LunarWispChase = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "Chase"
                                                    select x).First();
                    LunarWispChase.minDistance = 25f;
                    break;

                case "BisonMaster(Clone)":
                    AISkillDriver BisonCharge = (from x in masterm.GetComponents<AISkillDriver>()
                                                 where x.skillSlot == SkillSlot.Utility
                                                 select x).First();
                    BisonCharge.minDistance = 0f;
                    BisonCharge.maxDistance = 150f + (20f * AIScaling.Value * Run.instance.stageClearCount);
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "VultureMaster(Clone)":
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "MiniMushroomMaster(Clone)":
                    AISkillDriver MushrumPath = (from x in masterm.GetComponents<AISkillDriver>()
                                                 where x.customName == "Path"
                                                 select x).First();
                    MushrumPath.shouldSprint = true;
                    AISkillDriver PathStrafe = (from x in masterm.GetComponents<AISkillDriver>()
                                                where x.customName == "PathStrafe"
                                                select x).First();
                    PathStrafe.shouldSprint = true;
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "VagrantMaster(Clone)":
                    AISkillDriver VagrantChase = (from x in masterm.GetComponents<AISkillDriver>()
                                                  where x.customName == "Chase"
                                                  select x).First();
                    VagrantChase.minDistance = 25f;
                    break;

                case "TitanGoldMaster(Clone)":
                    AISkillDriver AurelioniteLaser = (from x in masterm.GetComponents<AISkillDriver>()
                                                      where x.skillSlot == SkillSlot.Special
                                                      select x).First();
                    AurelioniteLaser.maxUserHealthFraction = Mathf.Infinity;
                    AurelioniteLaser.minDistance = 16f;
                    AurelioniteLaser.movementType = AISkillDriver.MovementType.StrafeMovetarget;

                    AISkillDriver AurelioniteTurret = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.skillSlot == SkillSlot.Utility
                                                       select x).First();
                    AurelioniteTurret.maxUserHealthFraction = 0.8f;
                    AurelioniteTurret.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    break;

                case "HermitCrabMaster(Clone)":
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "LunarGolemMaster(Clone)":
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;

                case "VoidMegaCrabBody(Clone)":
                    master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    break;
            }
        }

        internal static void ApplyHooks()
        {
            On.EntityStates.BasicMeleeAttack.OnEnter += GupSpikesState;
            On.EntityStates.BeetleGuardMonster.FireSunder.OnEnter += FireSunder;
            On.EntityStates.BeetleGuardMonster.GroundSlam.OnEnter += GroundSlam;
            On.EntityStates.BeetleMonster.HeadbuttState.FixedUpdate += HeadbuttState;
            On.EntityStates.BeetleQueenMonster.FireSpit.OnEnter += FireSpit;
            On.EntityStates.BeetleQueenMonster.SpawnWards.OnEnter += SpawnWards;
            On.EntityStates.Bison.Charge.OnEnter += Charge;
            On.EntityStates.Bison.Headbutt.OnEnter += Headbutt;
            On.EntityStates.BrotherHaunt.FireRandomProjectiles.OnEnter += FireRandomProjectiles;
            On.EntityStates.BrotherMonster.BaseSlideState.OnEnter += BaseSlideState;
            On.EntityStates.BrotherMonster.ExitSkyLeap.OnEnter += ExitSkyLeap;
            On.EntityStates.BrotherMonster.FistSlam.OnEnter += FistSlam;
            On.EntityStates.BrotherMonster.HoldSkyLeap.OnEnter += HoldSkyLeap;
            On.EntityStates.BrotherMonster.SpellChannelEnterState.OnEnter += SpellChannelEnterState;
            On.EntityStates.BrotherMonster.SpellChannelExitState.OnEnter += SpellChannelExitState;
            On.EntityStates.BrotherMonster.SpellChannelState.OnEnter += SpellChannelState;
            On.EntityStates.BrotherMonster.SprintBash.OnEnter += SprintBash;
            On.EntityStates.BrotherMonster.StaggerEnter.OnEnter += StaggerEnter;
            On.EntityStates.BrotherMonster.StaggerExit.OnEnter += StaggerExit;
            On.EntityStates.BrotherMonster.StaggerLoop.OnEnter += StaggerLoop;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState;
            On.EntityStates.BrotherMonster.UltChannelState.OnEnter += UltChannelState;
            On.EntityStates.BrotherMonster.Weapon.FireLunarShards.OnEnter += FireLunarShards;
            On.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate += WeaponSlam2;
            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter += WeaponSlam;
            On.EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.OnEnter += ChargeBombardment;
            On.EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.OnEnter += FireBombardment;
            On.EntityStates.ClayBoss.FireTarball.OnEnter += FireTarBall;
            On.EntityStates.ClayBoss.PrepTarBall.OnEnter += PrepTarBall;
            On.EntityStates.ClayBruiser.Weapon.FireSonicBoom.OnEnter += FireSonicBoom;
            On.EntityStates.GenericCharacterSpawnState.OnEnter += GenericCharacterSpawnState;
            On.EntityStates.GolemMonster.ChargeLaser.OnEnter += ChargeLaser;
            On.EntityStates.GolemMonster.ClapState.OnEnter += ClapState;
            On.EntityStates.GolemMonster.FireLaser.OnEnter += FireLaser;
            On.EntityStates.GrandParentBoss.GroundSwipe.OnEnter += GroundSwipe;
            On.EntityStates.GravekeeperBoss.FireHook.OnEnter += FireHook;
            On.EntityStates.GravekeeperBoss.PrepHook.OnEnter += PrepHook;
            On.EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.OnEnter += GravekeeperBarrage;
            On.EntityStates.GreaterWispMonster.ChargeCannons.OnEnter += ChargeCannons;
            On.EntityStates.HermitCrab.FireMortar.OnEnter += FireMortar;
            On.EntityStates.ImpBossMonster.BlinkState.OnEnter += BlinkState2;
            On.EntityStates.ImpBossMonster.FireVoidspikes.OnEnter += FireVoidspikes;
            On.EntityStates.ImpBossMonster.GroundPound.OnEnter += GroundPound;
            On.EntityStates.ImpMonster.BlinkState.OnEnter += BlinkState;
            On.EntityStates.ImpMonster.DoubleSlash.OnEnter += DoubleSlash;
            On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter += FireMegaFireball;
            On.EntityStates.LemurianBruiserMonster.Flamebreath.OnEnter += Flamebreath;
            On.EntityStates.LemurianMonster.Bite.FixedUpdate += Bite2;
            On.EntityStates.LemurianMonster.Bite.OnEnter += Bite1;
            On.EntityStates.LemurianMonster.ChargeFireball.OnEnter += ChargeFireball;
            On.EntityStates.LemurianMonster.FireFireball.OnEnter += FireFireball;
            On.EntityStates.LunarWisp.FireLunarGuns.OnEnter += FireLunarGuns;
            On.EntityStates.MajorConstruct.Weapon.FireLaser.OnEnter += FireLaser;
            On.EntityStates.MiniMushroom.SporeGrenade.OnEnter += SporeGrenade;
            On.EntityStates.MinorConstruct.Weapon.ChargeConstructBeam.OnEnter += ChargeConstructBeam;
            On.EntityStates.MinorConstruct.Weapon.FireConstructBeam.OnEnter += FireConstructBeam;
            On.EntityStates.Missions.BrotherEncounter.Phase2.OnEnter += Phase2;
            On.EntityStates.Missions.BrotherEncounter.Phase3.OnEnter += Phase3;
            On.EntityStates.NullifierMonster.FirePortalBomb.OnEnter += FirePortalBomb;
            On.EntityStates.TitanMonster.FireFist.OnEnter += FireFist2;
            On.EntityStates.TitanMonster.FireFist.OnEnter += FireFist;
            On.EntityStates.TitanMonster.FireGoldFist.PlacePredictedAttack += FireGoldFist;
            On.EntityStates.VagrantMonster.ChargeTrackingBomb.OnEnter += ChargeTrackingBomb;
            On.EntityStates.VoidBarnacle.Weapon.ChargeFire.OnEnter += ChargeFire;
            On.EntityStates.VoidBarnacle.Weapon.Fire.OnEnter += Fire;
            On.EntityStates.NullifierMonster.DeathState.OnEnter += DeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter += DeathState2;

            On.RoR2.Projectile.ProjectileSimple.Start += SpeedUpProjectiles;

            On.RoR2.Run.RecalculateDifficultyCoefficentInternal += AmbientLevelBoost;
            On.RoR2.MasterSummon.Perform += IncreaseMonsterCap;
            On.RoR2.CombatDirector.Awake += CombatDirector_Awake;
            On.RoR2.HoldoutZoneController.Awake += HoldoutZoneController_Awake;

            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter -= CleanupPillar;
            On.EntityStates.NullifierMonster.DeathState.OnEnter -= CleanupDeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter -= CleanupDeathState2;
        }

        internal static void UndoHooks()
        {
            On.EntityStates.BasicMeleeAttack.OnEnter -= GupSpikesState;
            On.EntityStates.BeetleGuardMonster.FireSunder.OnEnter -= FireSunder;
            On.EntityStates.BeetleGuardMonster.GroundSlam.OnEnter -= GroundSlam;
            On.EntityStates.BeetleMonster.HeadbuttState.FixedUpdate -= HeadbuttState;
            On.EntityStates.BeetleQueenMonster.FireSpit.OnEnter -= FireSpit;
            On.EntityStates.BeetleQueenMonster.SpawnWards.OnEnter -= SpawnWards;
            On.EntityStates.Bison.Charge.OnEnter -= Charge;
            On.EntityStates.Bison.Headbutt.OnEnter -= Headbutt;
            On.EntityStates.BrotherHaunt.FireRandomProjectiles.OnEnter -= FireRandomProjectiles;
            On.EntityStates.BrotherMonster.BaseSlideState.OnEnter -= BaseSlideState;
            On.EntityStates.BrotherMonster.ExitSkyLeap.OnEnter -= ExitSkyLeap;
            On.EntityStates.BrotherMonster.FistSlam.OnEnter -= FistSlam;
            On.EntityStates.BrotherMonster.HoldSkyLeap.OnEnter -= HoldSkyLeap;
            On.EntityStates.BrotherMonster.SpellChannelEnterState.OnEnter -= SpellChannelEnterState;
            On.EntityStates.BrotherMonster.SpellChannelExitState.OnEnter -= SpellChannelExitState;
            On.EntityStates.BrotherMonster.SpellChannelState.OnEnter -= SpellChannelState;
            On.EntityStates.BrotherMonster.SprintBash.OnEnter -= SprintBash;
            On.EntityStates.BrotherMonster.StaggerEnter.OnEnter -= StaggerEnter;
            On.EntityStates.BrotherMonster.StaggerExit.OnEnter -= StaggerExit;
            On.EntityStates.BrotherMonster.StaggerLoop.OnEnter -= StaggerLoop;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= TrueDeathState;
            On.EntityStates.BrotherMonster.UltChannelState.OnEnter -= UltChannelState;
            On.EntityStates.BrotherMonster.Weapon.FireLunarShards.OnEnter -= FireLunarShards;
            On.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate -= WeaponSlam2;
            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter -= WeaponSlam;
            On.EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.OnEnter -= ChargeBombardment;
            On.EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.OnEnter -= FireBombardment;
            On.EntityStates.ClayBoss.FireTarball.OnEnter -= FireTarBall;
            On.EntityStates.ClayBoss.PrepTarBall.OnEnter -= PrepTarBall;
            On.EntityStates.ClayBruiser.Weapon.FireSonicBoom.OnEnter -= FireSonicBoom;
            On.EntityStates.GenericCharacterSpawnState.OnEnter -= GenericCharacterSpawnState;
            On.EntityStates.GolemMonster.ChargeLaser.OnEnter -= ChargeLaser;
            On.EntityStates.GolemMonster.ClapState.OnEnter -= ClapState;
            On.EntityStates.GolemMonster.FireLaser.OnEnter -= FireLaser;
            On.EntityStates.GrandParentBoss.GroundSwipe.OnEnter -= GroundSwipe;
            On.EntityStates.GravekeeperBoss.FireHook.OnEnter -= FireHook;
            On.EntityStates.GravekeeperBoss.PrepHook.OnEnter -= PrepHook;
            On.EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.OnEnter -= GravekeeperBarrage;
            On.EntityStates.GreaterWispMonster.ChargeCannons.OnEnter -= ChargeCannons;
            On.EntityStates.HermitCrab.FireMortar.OnEnter -= FireMortar;
            On.EntityStates.ImpBossMonster.BlinkState.OnEnter -= BlinkState2;
            On.EntityStates.ImpBossMonster.FireVoidspikes.OnEnter -= FireVoidspikes;
            On.EntityStates.ImpBossMonster.GroundPound.OnEnter -= GroundPound;
            On.EntityStates.ImpMonster.BlinkState.OnEnter -= BlinkState;
            On.EntityStates.ImpMonster.DoubleSlash.OnEnter -= DoubleSlash;
            On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter -= FireMegaFireball;
            On.EntityStates.LemurianBruiserMonster.Flamebreath.OnEnter -= Flamebreath;
            On.EntityStates.LemurianMonster.Bite.FixedUpdate -= Bite2;
            On.EntityStates.LemurianMonster.Bite.OnEnter -= Bite1;
            On.EntityStates.LemurianMonster.ChargeFireball.OnEnter -= ChargeFireball;
            On.EntityStates.LemurianMonster.FireFireball.OnEnter -= FireFireball;
            On.EntityStates.LunarWisp.FireLunarGuns.OnEnter -= FireLunarGuns;
            On.EntityStates.MajorConstruct.Weapon.FireLaser.OnEnter -= FireLaser;
            On.EntityStates.MiniMushroom.SporeGrenade.OnEnter -= SporeGrenade;
            On.EntityStates.MinorConstruct.Weapon.ChargeConstructBeam.OnEnter -= ChargeConstructBeam;
            On.EntityStates.MinorConstruct.Weapon.FireConstructBeam.OnEnter -= FireConstructBeam;
            On.EntityStates.Missions.BrotherEncounter.Phase2.OnEnter -= Phase2;
            On.EntityStates.Missions.BrotherEncounter.Phase3.OnEnter -= Phase3;
            On.EntityStates.NullifierMonster.FirePortalBomb.OnEnter -= FirePortalBomb;
            On.EntityStates.TitanMonster.FireFist.OnEnter -= FireFist2;
            On.EntityStates.TitanMonster.FireFist.OnEnter -= FireFist;
            On.EntityStates.TitanMonster.FireGoldFist.PlacePredictedAttack -= FireGoldFist;
            On.EntityStates.VagrantMonster.ChargeTrackingBomb.OnEnter -= ChargeTrackingBomb;
            On.EntityStates.VoidBarnacle.Weapon.ChargeFire.OnEnter -= ChargeFire;
            On.EntityStates.VoidBarnacle.Weapon.Fire.OnEnter -= Fire;
            On.EntityStates.NullifierMonster.DeathState.OnEnter -= DeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter -= DeathState2;

            On.RoR2.Projectile.ProjectileSimple.Start -= SpeedUpProjectiles;

            On.RoR2.Run.RecalculateDifficultyCoefficentInternal -= AmbientLevelBoost;
            On.RoR2.MasterSummon.Perform -= IncreaseMonsterCap;
            On.RoR2.CombatDirector.Awake -= CombatDirector_Awake;
            On.RoR2.HoldoutZoneController.Awake -= HoldoutZoneController_Awake;

            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter += CleanupPillar;
            On.EntityStates.NullifierMonster.DeathState.OnEnter += CleanupDeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter += CleanupDeathState2;
        }

        #region OnHooks

        private static void CombatDirector_Awake(On.RoR2.CombatDirector.orig_Awake orig, CombatDirector self)
        {
            if (EnableCDirector.Value && !RiskyModCompat.enabled)
            {
                self.maxConsecutiveCheapSkips = 2;
                self.eliteBias = 0.9f;
                self.maxSpawnDistance = 80f;
                self.minSpawnRange = 14f;
                self.creditMultiplier = 1f + (CreditMultiplier.Value * Run.instance.stageClearCount);
                self.goldRewardCoefficient = 0.94f / self.creditMultiplier;
            }
            orig(self);
        }

        private static CharacterMaster IncreaseMonsterCap(On.RoR2.MasterSummon.orig_Perform orig, MasterSummon self)
        {
            TeamIndex teamIndex = TeamIndex.Monster;
            TeamDef teamDef = TeamCatalog.GetTeamDef(teamIndex);
            if (teamDef != null)
            {
                teamDef.softCharacterLimit = MonsterLimit.Value;
            }
            return orig(self);
        }

        private static void CleanupDeathState(On.EntityStates.NullifierMonster.DeathState.orig_OnEnter orig, EntityStates.NullifierMonster.DeathState self)
        {
            EntityStates.NullifierMonster.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 3f;
            orig(self);
        }

        private static void CleanupDeathState2(On.EntityStates.VoidMegaCrab.DeathState.orig_OnEnter orig, EntityStates.VoidMegaCrab.DeathState self)
        {
            EntityStates.VoidMegaCrab.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 5f;
            orig(self);
        }

        private static void DeathState(On.EntityStates.NullifierMonster.DeathState.orig_OnEnter orig, EntityStates.NullifierMonster.DeathState self)
        {
            EntityStates.NullifierMonster.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 2.6f / self.attackSpeedStat;
            orig(self);
        }

        private static void DeathState2(On.EntityStates.VoidMegaCrab.DeathState.orig_OnEnter orig, EntityStates.VoidMegaCrab.DeathState self)
        {
            EntityStates.VoidMegaCrab.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 3.8f / self.attackSpeedStat;
            orig(self);
        }

        private static void FireHook(On.EntityStates.GravekeeperBoss.FireHook.orig_OnEnter orig, EntityStates.GravekeeperBoss.FireHook self)
        {
            EntityStates.GravekeeperBoss.FireHook.projectilePrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/GravekeeperHookProjectile");
            EntityStates.GravekeeperBoss.FireHook.projectileDamageCoefficient = 1.2f;
            EntityStates.GravekeeperBoss.FireHook.baseDuration = 1.5f;
            EntityStates.GravekeeperBoss.FireHook.projectileCount = 40;
            EntityStates.GravekeeperBoss.FireHook.spread = 360f;
            orig(self);
        }

        private static void SpawnWards(On.EntityStates.BeetleQueenMonster.SpawnWards.orig_OnEnter orig, EntityStates.BeetleQueenMonster.SpawnWards self)
        {
            EntityStates.BeetleQueenMonster.SpawnWards.baseDuration = 3.5f;
            EntityStates.BeetleQueenMonster.SpawnWards.orbCountMax = 10;
            EntityStates.BeetleQueenMonster.SpawnWards.orbRange = 100f;
            orig(self);
        }

        private static void FireSpit(On.EntityStates.BeetleQueenMonster.FireSpit.orig_OnEnter orig, EntityStates.BeetleQueenMonster.FireSpit self)
        {
            EntityStates.BeetleQueenMonster.FireSpit.baseDuration = 1.5f;
            orig(self);
        }

        private static void PrepHook(On.EntityStates.GravekeeperBoss.PrepHook.orig_OnEnter orig, EntityStates.GravekeeperBoss.PrepHook self)
        {
            EntityStates.GravekeeperBoss.PrepHook.baseDuration = 1.3f;
            orig(self);
        }

        private static void GravekeeperBarrage(On.EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.orig_OnEnter orig, EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage self)
        {
            EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.damageCoefficient = 2f;
            EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.missileSpawnDelay = 0.15f;
            EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.missileSpawnFrequency = 5f;
            orig(self);
        }

        private static void HoldSkyLeap(On.EntityStates.BrotherMonster.HoldSkyLeap.orig_OnEnter orig, EntityStates.BrotherMonster.HoldSkyLeap self)
        {
            if (MithrixAS.Value)
            {
                EntityStates.BrotherMonster.HoldSkyLeap.duration = 2f / self.attackSpeedStat;
            }
            else
            {
                EntityStates.BrotherMonster.HoldSkyLeap.duration = 2f;
            }

            orig(self);
        }

        private static void ExitSkyLeap(On.EntityStates.BrotherMonster.ExitSkyLeap.orig_OnEnter orig, EntityStates.BrotherMonster.ExitSkyLeap self)
        {
            EntityStates.BrotherMonster.ExitSkyLeap.waveProjectileCount = 20;
            EntityStates.BrotherMonster.ExitSkyLeap.waveProjectileDamageCoefficient = 1.2f;
            orig(self);
        }

        private static void WeaponSlam(On.EntityStates.BrotherMonster.WeaponSlam.orig_OnEnter orig, EntityStates.BrotherMonster.WeaponSlam self)
        {
            if (MithrixAS.Value)
            {
                EntityStates.BrotherMonster.WeaponSlam.duration = 3f / self.attackSpeedStat;
            }
            else
            {
                EntityStates.BrotherMonster.WeaponSlam.duration = 3f;
            }
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileArc = 360f;
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileCount = 8;
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileDamageCoefficient = 1.3f;
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileForce = -1300f;
            EntityStates.BrotherMonster.WeaponSlam.weaponForce = -2000f;
            var pillarprefab = EntityStates.BrotherMonster.WeaponSlam.pillarProjectilePrefab;
            pillarprefab.transform.localScale = new Vector3(4f, 4f, 4f);
            var ghost = pillarprefab.GetComponent<ProjectileController>().ghostPrefab;
            ghost.transform.localScale = new Vector3(4f, 4f, 4f);
            hasFired = false;
            orig(self);
        }

        private static void BaseSlideState(On.EntityStates.BrotherMonster.BaseSlideState.orig_OnEnter orig, EntityStates.BrotherMonster.BaseSlideState self)
        {
            if (MithrixAS.Value)
            {
                EntityStates.BrotherMonster.BaseSlideState.duration /= self.attackSpeedStat;
            }
            if (self is EntityStates.BrotherMonster.SlideBackwardState)
            {
                self.slideRotation = Quaternion.identity;
            }
            orig(self);
        }

        private static void SprintBash(On.EntityStates.BrotherMonster.SprintBash.orig_OnEnter orig, EntityStates.BrotherMonster.SprintBash self)
        {
            EntityStates.BrotherMonster.SprintBash.durationBeforePriorityReduces = 0.18f;
            if (MithrixAS.Value)
            {
                self.baseDuration = 1.4f / self.attackSpeedStat;
            }
            else
            {
                self.baseDuration = 1.4f;
            }
            self.damageCoefficient = 1.4f;
            self.pushAwayForce = 1500f;
            self.forceVector = new Vector3(0f, 750f, 0f);
            orig(self);
        }

        private static void FireLunarShards(On.EntityStates.BrotherMonster.Weapon.FireLunarShards.orig_OnEnter orig, EntityStates.BrotherMonster.Weapon.FireLunarShards self)
        {
            EntityStates.BrotherMonster.Weapon.FireLunarShards.spreadBloomValue = 20f;
            EntityStates.BrotherMonster.Weapon.FireLunarShards.recoilAmplitude = 2f;
            EntityStates.BrotherMonster.Weapon.FireLunarShards.baseDuration = 0.03f;
            orig(self);
        }

        private static void Phase2(On.EntityStates.Missions.BrotherEncounter.Phase2.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase2 self)
        {
            orig(self);
            self.PreEncounterBegin();
            self.outer.SetNextState(new EntityStates.Missions.BrotherEncounter.Phase3());
        }

        private static void FistSlam(On.EntityStates.BrotherMonster.FistSlam.orig_OnEnter orig, EntityStates.BrotherMonster.FistSlam self)
        {
            EntityStates.BrotherMonster.FistSlam.waveProjectileDamageCoefficient = 2.3f;
            EntityStates.BrotherMonster.FistSlam.healthCostFraction = 0f;
            EntityStates.BrotherMonster.FistSlam.waveProjectileCount = 20;
            EntityStates.BrotherMonster.FistSlam.baseDuration = 3.5f;
            if (self.isAuthority)
            {
                float waves = 8f;
                float SpinnyCount = 360f / waves;
                Vector3 plane = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                Transform transgender = self.FindModelChild(EntityStates.BrotherMonster.WeaponSlam.muzzleString);
                Vector3 pos = transgender.position + new Vector3(Random.Range(-50f, 50f), 0f, Random.Range(-50f, 50f));
                int l = 0;
                while (l < waves)
                {
                    Vector3 zase = Quaternion.AngleAxis(SpinnyCount * l, Vector3.up) * plane;
                    ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.UltChannelState.waveProjectileLeftPrefab, pos, Util.QuaternionSafeLookRotation(zase), self.gameObject, self.characterBody.damage * 2f, EntityStates.BrotherMonster.FistSlam.waveProjectileForce, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), 0, null, -1f);
                    l++;
                }
            }
            orig(self);
        }

        private static void SpellChannelEnterState(On.EntityStates.BrotherMonster.SpellChannelEnterState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelEnterState self)
        {
            EntityStates.BrotherMonster.SpellChannelEnterState.duration = 3f;
            orig(self);
        }

        private static void SpellChannelState(On.EntityStates.BrotherMonster.SpellChannelState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelState self)
        {
            EntityStates.BrotherMonster.SpellChannelState.stealInterval = 0.05f;
            EntityStates.BrotherMonster.SpellChannelState.delayBeforeBeginningSteal = 0f;
            EntityStates.BrotherMonster.SpellChannelState.maxDuration = 15f;
            orig(self);
        }

        private static void SpellChannelExitState(On.EntityStates.BrotherMonster.SpellChannelExitState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelExitState self)
        {
            EntityStates.BrotherMonster.SpellChannelExitState.lendInterval = 0.04f;
            EntityStates.BrotherMonster.SpellChannelExitState.duration = 2.5f;
            orig(self);
        }

        private static void StaggerEnter(On.EntityStates.BrotherMonster.StaggerEnter.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerEnter self)
        {
            self.duration = 0f;
            orig(self);
        }

        private static void StaggerExit(On.EntityStates.BrotherMonster.StaggerExit.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerExit self)
        {
            self.duration = 0f;
            orig(self);
        }

        private static void StaggerLoop(On.EntityStates.BrotherMonster.StaggerLoop.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerLoop self)
        {
            self.duration = 0f;
            orig(self);
        }

        private static void TrueDeathState(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self)
        {
            EntityStates.BrotherMonster.TrueDeathState.dissolveDuration = 5f;
            orig(self);
        }

        private static void FireRandomProjectiles(On.EntityStates.BrotherHaunt.FireRandomProjectiles.orig_OnEnter orig, EntityStates.BrotherHaunt.FireRandomProjectiles self)
        {
            EntityStates.BrotherHaunt.FireRandomProjectiles.maximumCharges = 150;
            EntityStates.BrotherHaunt.FireRandomProjectiles.chargeRechargeDuration = 0.03f;
            EntityStates.BrotherHaunt.FireRandomProjectiles.chanceToFirePerSecond = 0.5f;
            EntityStates.BrotherHaunt.FireRandomProjectiles.damageCoefficient = 12f;
            orig(self);
        }

        private static void FireFist(On.EntityStates.TitanMonster.FireFist.orig_OnEnter orig, EntityStates.TitanMonster.FireFist self)
        {
            EntityStates.TitanMonster.FireFist.entryDuration = 1.6f;
            EntityStates.TitanMonster.FireFist.fireDuration = 1.1f;
            EntityStates.TitanMonster.FireFist.exitDuration = 1.6f;
            EntityStates.TitanMonster.FireFist.fistDamageCoefficient = 1.2f;
            orig(self);
        }

        private static void HeadbuttState(On.EntityStates.BeetleMonster.HeadbuttState.orig_FixedUpdate orig, EntityStates.BeetleMonster.HeadbuttState self)
        {
            EntityStates.BeetleMonster.HeadbuttState.baseDuration = 2.7f;
            if (self.modelAnimator && self.modelAnimator.GetFloat("Headbutt.hitBoxActive") > 0.5f)
            {
                Vector3 direction = self.GetAimRay().direction;
                Vector3 a = direction.normalized * 2f * self.moveSpeedStat;
                Vector3 b = Vector3.up * 5f;
                Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * 4.5f;
                self.characterMotor.Motor.ForceUnground();
                self.characterMotor.velocity = a + b + b2;
            }
            if (self.fixedAge > 0.5f && self.fixedAge < 2.1f)
            {
                self.attack.Fire(null);
            }
            orig(self);
        }

        private static void GenericCharacterSpawnState(On.EntityStates.GenericCharacterSpawnState.orig_OnEnter orig, EntityStates.GenericCharacterSpawnState self)
        {
            orig(self);
            if (self.duration > 2.5f)
            {
                self.duration = 2.5f;
            }
        }

        private static void FireSunder(On.EntityStates.BeetleGuardMonster.FireSunder.orig_OnEnter orig, EntityStates.BeetleGuardMonster.FireSunder self)
        {
            EntityStates.BeetleGuardMonster.FireSunder.baseDuration = 1.35f;
            orig(self);
        }

        private static void GroundSlam(On.EntityStates.BeetleGuardMonster.GroundSlam.orig_OnEnter orig, EntityStates.BeetleGuardMonster.GroundSlam self)
        {
            EntityStates.BeetleGuardMonster.GroundSlam.baseDuration = 2.2f;
            orig(self);
        }

        private static void Headbutt(On.EntityStates.Bison.Headbutt.orig_OnEnter orig, EntityStates.Bison.Headbutt self)
        {
            EntityStates.Bison.Headbutt.baseHeadbuttDuration = 2.5f;
            orig(self);
        }

        private static void Charge(On.EntityStates.Bison.Charge.orig_OnEnter orig, EntityStates.Bison.Charge self)
        {
            EntityStates.Bison.Charge.chargeMovementSpeedCoefficient = 12f;
            EntityStates.Bison.Charge.turnSpeed = 99999f;
            EntityStates.Bison.Charge.turnSmoothTime = 0f;
            EntityStates.Bison.Charge.selfStunDuration = 0.5f;
            orig(self);
        }

        private static void FireSonicBoom(On.EntityStates.ClayBruiser.Weapon.FireSonicBoom.orig_OnEnter orig, EntityStates.ClayBruiser.Weapon.FireSonicBoom self)
        {
            self.baseDuration = 0.8f;
            self.fieldOfView = 90f;
            orig(self);
        }

        private static void FireMegaFireball(On.EntityStates.LemurianBruiserMonster.FireMegaFireball.orig_OnEnter orig, EntityStates.LemurianBruiserMonster.FireMegaFireball self)
        {
            EntityStates.LemurianBruiserMonster.FireMegaFireball.projectileCount = 10;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.totalYawSpread = 90f;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.baseFireDuration = 0.5f;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.projectileSpeed = 50f;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.damageCoefficient = 1.75f;
            orig(self);
        }

        private static void ChargeCannons(On.EntityStates.GreaterWispMonster.ChargeCannons.orig_OnEnter orig, EntityStates.GreaterWispMonster.ChargeCannons self)
        {
            self.baseDuration = 1.25f;
            orig(self);
        }

        private static void Bite1(On.EntityStates.LemurianMonster.Bite.orig_OnEnter orig, EntityStates.LemurianMonster.Bite self)
        {
            EntityStates.LemurianMonster.Bite.radius = 3f;
            EntityStates.LemurianMonster.Bite.baseDuration = 0.8f;
            EntityStates.LemurianMonster.Bite.forceMagnitude = 400f;
            orig(self);
        }

        private static void Bite2(On.EntityStates.LemurianMonster.Bite.orig_FixedUpdate orig, EntityStates.LemurianMonster.Bite self)
        {
            if (self.modelAnimator && self.modelAnimator.GetFloat("Bite.hitBoxActive") > 0.1f)
            {
                Vector3 direction = self.GetAimRay().direction;
                Vector3 a = direction.normalized * 1.5f * self.moveSpeedStat;
                Vector3 b = new Vector3(direction.x, 0f, direction.z).normalized * 1.5f;
                self.characterMotor.Motor.ForceUnground();
                self.characterMotor.velocity = a + b;
            }
            if (self.fixedAge > 0.5f)
            {
                self.attack.Fire(null);
            }
            orig(self);
        }

        private static void ChargeFireball(On.EntityStates.LemurianMonster.ChargeFireball.orig_OnEnter orig, EntityStates.LemurianMonster.ChargeFireball self)
        {
            EntityStates.LemurianMonster.ChargeFireball.baseDuration = 0.48f;
            orig(self);
        }

        private static void ClapState(On.EntityStates.GolemMonster.ClapState.orig_OnEnter orig, EntityStates.GolemMonster.ClapState self)
        {
            EntityStates.GolemMonster.ClapState.damageCoefficient = 3.5f;
            EntityStates.GolemMonster.ClapState.radius = 6f;
            EntityStates.GolemMonster.ClapState.duration = 1.6f;
            orig(self);
        }

        private static void ChargeLaser(On.EntityStates.GolemMonster.ChargeLaser.orig_OnEnter orig, EntityStates.GolemMonster.ChargeLaser self)
        {
            EntityStates.GolemMonster.ChargeLaser.baseDuration = 1.75f;
            orig(self);
        }

        private static void FireLaser(On.EntityStates.GolemMonster.FireLaser.orig_OnEnter orig, EntityStates.GolemMonster.FireLaser self)
        {
            EntityStates.GolemMonster.FireLaser.damageCoefficient = 2.5f;
            EntityStates.GolemMonster.FireLaser.blastRadius = 6f;
            orig(self);
        }

        private static void FireFireball(On.EntityStates.LemurianMonster.FireFireball.orig_OnEnter orig, EntityStates.LemurianMonster.FireFireball self)
        {
            EntityStates.LemurianMonster.FireFireball.damageCoefficient = 1.2f;
            orig(self);
        }

        private static void SporeGrenade(On.EntityStates.MiniMushroom.SporeGrenade.orig_OnEnter orig, EntityStates.MiniMushroom.SporeGrenade self)
        {
            EntityStates.MiniMushroom.SporeGrenade.baseChargeTime = 1.25f;
            EntityStates.MiniMushroom.SporeGrenade.baseDuration = 1.25f;
            EntityStates.MiniMushroom.SporeGrenade.damageCoefficient = 1.1f;
            orig(self);
        }

        private static void ChargeFire(On.EntityStates.VoidBarnacle.Weapon.ChargeFire.orig_OnEnter orig, EntityStates.VoidBarnacle.Weapon.ChargeFire self)
        {
            self.baseDuration = 0.7f;
            orig(self);
        }

        private static void Fire(On.EntityStates.VoidBarnacle.Weapon.Fire.orig_OnEnter orig, EntityStates.VoidBarnacle.Weapon.Fire self)
        {
            self.numberOfFireballs = 2;
            orig(self);
        }

        private static void PrepTarBall(On.EntityStates.ClayBoss.PrepTarBall.orig_OnEnter orig, EntityStates.ClayBoss.PrepTarBall self)
        {
            EntityStates.ClayBoss.PrepTarBall.baseDuration = 1.8f;
            orig(self);
        }

        private static void FireTarBall(On.EntityStates.ClayBoss.FireTarball.orig_OnEnter orig, EntityStates.ClayBoss.FireTarball self)
        {
            EntityStates.ClayBoss.FireTarball.tarballCountMax = 5;
            EntityStates.ClayBoss.FireTarball.baseTimeBetweenShots = 0.7f;
            EntityStates.ClayBoss.FireTarball.cooldownDuration = 1.8f;
            orig(self);
        }

        private static void GupSpikesState(On.EntityStates.BasicMeleeAttack.orig_OnEnter orig, EntityStates.BasicMeleeAttack self)
        {
            if (self is EntityStates.Gup.GupSpikesState)
            {
                self.damageCoefficient = 2.8f;
            }
            orig(self);
        }

        private static void FireLunarGuns(On.EntityStates.LunarWisp.FireLunarGuns.orig_OnEnter orig, EntityStates.LunarWisp.FireLunarGuns self)
        {
            EntityStates.LunarWisp.FireLunarGuns.baseDamagePerSecondCoefficient = 1.8f;
            orig(self);
        }

        private static void FirePortalBomb(On.EntityStates.NullifierMonster.FirePortalBomb.orig_OnEnter orig, EntityStates.NullifierMonster.FirePortalBomb self)
        {
            EntityStates.NullifierMonster.FirePortalBomb.baseDuration = 0.25f;
            EntityStates.NullifierMonster.FirePortalBomb.damageCoefficient = 1.5f;
            EntityStates.NullifierMonster.FirePortalBomb.portalBombCount = 4;
            orig(self);
        }

        private static void DoubleSlash(On.EntityStates.ImpMonster.DoubleSlash.orig_OnEnter orig, EntityStates.ImpMonster.DoubleSlash self)
        {
            EntityStates.ImpMonster.DoubleSlash.selfForce = 2000f;
            EntityStates.ImpMonster.DoubleSlash.walkSpeedPenaltyCoefficient = 1.2f;
            EntityStates.ImpMonster.DoubleSlash.damageCoefficient = 1.9f;
            orig(self);
        }

        private static void ChargeConstructBeam(On.EntityStates.MinorConstruct.Weapon.ChargeConstructBeam.orig_OnEnter orig, EntityStates.MinorConstruct.Weapon.ChargeConstructBeam self)
        {
            self.baseDuration = 0.33f;
            orig(self);
        }

        private static void FireConstructBeam(On.EntityStates.MinorConstruct.Weapon.FireConstructBeam.orig_OnEnter orig, EntityStates.MinorConstruct.Weapon.FireConstructBeam self)
        {
            self.baseDelayBeforeFiringProjectile = 0f;
            orig(self);
        }

        private static void FireLaser(On.EntityStates.MajorConstruct.Weapon.FireLaser.orig_OnEnter orig, EntityStates.MajorConstruct.Weapon.FireLaser self)
        {
            self.aimMaxSpeed = 15f;
            orig(self);
        }

        private static void BlinkState(On.EntityStates.ImpMonster.BlinkState.orig_OnEnter orig, EntityStates.ImpMonster.BlinkState self)
        {
            EntityStates.ImpMonster.BlinkState.blinkDistance = 18f;
            orig(self);
        }

        private static void FireMortar(On.EntityStates.HermitCrab.FireMortar.orig_OnEnter orig, EntityStates.HermitCrab.FireMortar self)
        {
            EntityStates.HermitCrab.FireMortar.mortarCount = 2;
            EntityStates.HermitCrab.FireMortar.mortarDamageCoefficient = 2.5f;
            EntityStates.HermitCrab.FireMortar.minimumDistance = 0f;
            EntityStates.HermitCrab.FireMortar.timeToTarget = 1.8f;
            EntityStates.HermitCrab.FireMortar.baseDuration = 0.7f;
            orig(self);
        }

        private static void GroundSwipe(On.EntityStates.GrandParentBoss.GroundSwipe.orig_OnEnter orig, EntityStates.GrandParentBoss.GroundSwipe self)
        {
            self.baseFireProjectileDelay = 1.5f;
            self.projectileHorizontalSpeed = 70f;
            orig(self);
        }

        private static void ChargeBombardment(On.EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.orig_OnEnter orig, EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment self)
        {
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.baseMaxChargeTime = 2.5f;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.maxCharges = 12;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.minGrenadeCount = 6;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.maxGrenadeCount = 30;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.baseTotalDuration = 3f;
            orig(self);
        }

        private static void FireBombardment(On.EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.orig_OnEnter orig, EntityStates.ClayBoss.ClayBossWeapon.FireBombardment self)
        {
            self.grenadeCountMax = 30;
            EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.damageCoefficient = 0.8f;
            orig(self);
        }

        private static void FireVoidspikes(On.EntityStates.ImpBossMonster.FireVoidspikes.orig_OnEnter orig, EntityStates.ImpBossMonster.FireVoidspikes self)
        {
            EntityStates.ImpBossMonster.FireVoidspikes.walkSpeedPenaltyCoefficient = 1.2f;
            EntityStates.ImpBossMonster.FireVoidspikes.projectileCount = 10;
            EntityStates.ImpBossMonster.FireVoidspikes.damageCoefficient = 0.2f;
            orig(self);
        }

        private static void GroundPound(On.EntityStates.ImpBossMonster.GroundPound.orig_OnEnter orig, EntityStates.ImpBossMonster.GroundPound self)
        {
            EntityStates.ImpBossMonster.GroundPound.blastAttackRadius = 13f;
            orig(self);
        }

        private static void ChargeTrackingBomb(On.EntityStates.VagrantMonster.ChargeTrackingBomb.orig_OnEnter orig, EntityStates.VagrantMonster.ChargeTrackingBomb self)
        {
            EntityStates.VagrantMonster.ChargeTrackingBomb.baseDuration = 1.6f;
            orig(self);
        }

        private static void SpeedUpProjectiles(On.RoR2.Projectile.ProjectileSimple.orig_Start orig, ProjectileSimple self)
        {
            if (self.rigidbody && !self.rigidbody.useGravity && self.gameObject.GetComponent<TeamFilter>().teamIndex == TeamIndex.Monster)
            {
                self.desiredForwardSpeed *= ProjectileSpeed.Value;
            }
            orig(self);
        }

        private static void FireFist2(On.EntityStates.TitanMonster.FireFist.orig_OnEnter orig, EntityStates.TitanMonster.FireFist self)
        {
            if (self is EntityStates.TitanMonster.FireGoldFist)
            {
                EntityStates.TitanMonster.FireFist.entryDuration = 1.6f;
                EntityStates.TitanMonster.FireFist.fireDuration = 1.1f;
                EntityStates.TitanMonster.FireFist.exitDuration = 1.6f;
            }
            orig(self);
        }

        private static void FireGoldFist(On.EntityStates.TitanMonster.FireGoldFist.orig_PlacePredictedAttack orig, EntityStates.TitanMonster.FireGoldFist self)
        {
            EntityStates.TitanMonster.FireGoldFist.fistCount = 10;
            EntityStates.TitanMonster.FireGoldFist.delayBetweenFists = 0.05f;
            orig(self);
        }

        private static void BlinkState2(On.EntityStates.ImpBossMonster.BlinkState.orig_OnEnter orig, EntityStates.ImpBossMonster.BlinkState self)
        {
            self.blastAttackDamageCoefficient = 7f;
            self.duration = 2f;
            self.destinationAlertDuration = 1.7f;
            self.exitDuration = 1.3f;
            self.blinkDistance = 9999f;
            orig(self);
        }

        private static void Flamebreath(On.EntityStates.LemurianBruiserMonster.Flamebreath.orig_OnEnter orig, EntityStates.LemurianBruiserMonster.Flamebreath self)
        {
            EntityStates.LemurianBruiserMonster.Flamebreath.baseEntryDuration = 0.5f;
            EntityStates.LemurianBruiserMonster.Flamebreath.baseExitDuration = 0.2f;
            EntityStates.LemurianBruiserMonster.Flamebreath.baseFlamethrowerDuration = 1.8f;
            EntityStates.LemurianBruiserMonster.Flamebreath.radius = 6f;
            EntityStates.LemurianBruiserMonster.Flamebreath.maxSpread = 6f;
            Debug.Log("hey handsome");
            orig(self);
        }

        private static void WeaponSlam2(On.EntityStates.BrotherMonster.WeaponSlam.orig_FixedUpdate orig, EntityStates.BrotherMonster.WeaponSlam self)
        {
            if (self.isAuthority)
            {
                if (self.hasDoneBlastAttack)
                {
                    if (self.modelTransform)
                    {
                        if (!hasFired)
                        {
                            Ray ray = self.GetAimRay();
                            Transform transform = self.FindModelChild(EntityStates.BrotherMonster.Weapon.FireLunarShards.muzzleString);
                            if (transform)
                            {
                                ray.origin = transform.position;
                            }
                            var orbs = 5f;
                            hasFired = true;
                            float orbCount = 360f / orbs;
                            Vector3 plane = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                            Transform transgender = self.FindModelChild(EntityStates.BrotherMonster.WeaponSlam.muzzleString);
                            Vector3 pos = transgender.position;
                            int j = 0;
                            while (j < orbs)
                            {
                                Vector3 zase = Quaternion.AngleAxis(orbCount * j, Vector3.up) * plane;
                                ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.FistSlam.waveProjectilePrefab, pos, Util.QuaternionSafeLookRotation(zase), self.gameObject, self.characterBody.damage * EntityStates.BrotherMonster.FistSlam.waveProjectileDamageCoefficient * 0.3f, EntityStates.BrotherMonster.FistSlam.waveProjectileForce, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), 0, null, -1f);
                                j++;
                            }
                            // i hate this code
                        }
                    }
                }
            }
            orig(self);
        }

        private static void UltChannelState(On.EntityStates.BrotherMonster.UltChannelState.orig_OnEnter orig, EntityStates.BrotherMonster.UltChannelState self)
        {
            if (MithrixAS.Value)
            {
                EntityStates.BrotherMonster.UltChannelState.maxDuration = 5.8f / self.attackSpeedStat;
            }
            else
            {
                EntityStates.BrotherMonster.UltChannelState.maxDuration = 5.8f;
            }
            orig(self);
        }

        private static void Phase3(On.EntityStates.Missions.BrotherEncounter.Phase3.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase3 self)
        {
            /*
            if (NetworkServer.active)
            {
                foreach (TeamComponent teamComponent in new List<TeamComponent>(TeamComponent.GetTeamMembers(TeamIndex.Monster)))
                {
                    if (teamComponent)
                    {
                        HealthComponent component = teamComponent.GetComponent<HealthComponent>();
                        if (component)
                        {
                            component.Suicide(null, null, DamageType.Generic);
                        }
                    }
                }
            }
            // fucking hell ofc copied code from phase 4 start doesnt work
            */
            orig(self);
        }

        private static void CleanupPillar(On.EntityStates.BrotherMonster.WeaponSlam.orig_OnEnter orig, EntityStates.BrotherMonster.WeaponSlam self)
        {
            var pillarprefab = EntityStates.BrotherMonster.WeaponSlam.pillarProjectilePrefab;
            pillarprefab.transform.localScale = new Vector3(1f, 1f, 1f);
            var ghost = pillarprefab.GetComponent<ProjectileController>().ghostPrefab;
            ghost.transform.localScale = new Vector3(1f, 1f, 1f);
            orig(self);
        }

        private static void AmbientLevelBoost(On.RoR2.Run.orig_RecalculateDifficultyCoefficentInternal orig, Run self)
        {
            orig(self);
            try
            {
                self.ambientLevel = (LevelDiffBoostScaleWithPlayers.Value ? Mathf.Min(self.ambientLevel + LevelDiffBoost.Value, int.MaxValue) : Mathf.Min(self.ambientLevel + (LevelDiffBoost.Value / self.participatingPlayerCount), int.MaxValue));
            }
            catch
            {
                InfernoLogger.LogError("Failed to fully apply Level and Difficulty boost");
            }
        }

        private static void Ambient(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld<Run>("ambientLevelCap")
            ))
            {
                c.EmitDelegate<Func<float, float>>((levelIn) =>
                {
                    float difficultyBoost = LevelDiffBoost.Value;
                    Run.instance.compensatedDifficultyCoefficient += difficultyBoost * 0.05f; //stage 3 spawnrates at stage 0 monsoon, stage 2 spawnrates at stage 0 rainstorm
                    float levelOut = levelIn + difficultyBoost;
                    return levelOut;
                });
            }
            else
            {
                InfernoLogger.LogError("Failed to fully apply Level and Difficulty boost");
            }
        }

        private static void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self)
        {
            orig(self);
            self.calcRadius += Self_calcRadius;
            self.calcChargeRate += Self_calcChargeRate;
            self.calcColor += Self_calcColor;
        }

        private static void Self_calcColor(ref Color color)
        {
            color = new Color32(105, 30, 37, 255);
        }

        private static void Self_calcChargeRate(ref float rate)
        {
            rate *= 1f + TeleporterSpeed.Value / 100f;
        }

        private static void Self_calcRadius(ref float radius)
        {
            radius *= Mathf.Max(1f + TeleporterSize.Value / 100f, 0f);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.teamComponent && sender.teamComponent.teamIndex == TeamIndex.Monster)
            {
                args.armorAdd += LoopArmor.Value * Run.instance.loopClearCount;
            }
            if (sender && sender.teamComponent && sender.teamComponent.teamIndex == TeamIndex.Monster && sender.isBoss)
            {
                args.healthMultAdd += BossHp.Value * Run.instance.stageClearCount;
            }
            if (sender && sender.teamComponent && sender.teamComponent.teamIndex == TeamIndex.Monster && !sender.isBoss && !sender.isChampion)
            {
                args.healthMultAdd += MonsterHp.Value * Run.instance.stageClearCount;
            }
        }

        private void Wander_PickNewTargetLookPosition(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(0.5f),
                x => x.MatchLdcR4(4f)
            ))
            {
                c.Next.Operand = 0.1f;
                c.Index += 1;
                c.Next.Operand = 1f;
            }
            else
            {
                InfernoLogger.LogError("Failed to change AI Wander_PickNewTargetLookPosition");
            }
        }

        private void LookBusy_PickNewTargetLookDirection(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(0.5f),
                x => x.MatchLdcR4(4f)
            ))
            {
                c.Next.Operand = 0.1f;
                c.Index += 1;
                c.Next.Operand = 1f;
            }
            else
            {
                InfernoLogger.LogError("Failed to change AI LookBusy_PickNewTargetLookPosition");
            }
        }

        private void LookBusy_OnEnter(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(2f),
                x => x.MatchLdcR4(7f)
            ))
            {
                c.Next.Operand = 0.1f;
                c.Index += 1;
                c.Next.Operand = 1f;
            }
            else
            {
                InfernoLogger.LogError("Failed to change AI LookBusy_OnEnter");
            }
        }

        #endregion OnHooks
    }

    public class BasePerSurvivorClearGameInfernoAchievement : BaseAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += OnClientGameOverGlobal;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= OnClientGameOverGlobal;
            base.OnBodyRequirementBroken();
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if (!runReport.gameEnding)
            {
                return;
            }
            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
                if (difficultyDef != null && difficultyDef == Main.InfernoDiffDef)
                {
                    runReport.gameEnding.lunarCoinReward = 15;
                    runReport.gameEnding.showCredits = false;
                    Grant();
                }
            }
        }
    }

    [RegisterAchievement("CommandoClearGameInferno", "Skins.Inferno_Commando", null, null)]
    public class CommandoClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CommandoBody");
        }
    }

    [RegisterAchievement("Bandit2ClearGameInferno", "Skins.Inferno_Bandit", "CompleteThreeStages", null)]
    public class BanditClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("Bandit2Body");
        }
    }

    [RegisterAchievement("CaptainClearGameInferno", "Skins.Inferno_Captain", "CompleteMainEnding", null)]
    public class CaptainClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CaptainBody");
        }
    }

    public class InfernoPostProcessingController : MonoBehaviour
    {
        public PostProcessVolume volume;

        public void Start()
        {
            volume = GetComponent<PostProcessVolume>();
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