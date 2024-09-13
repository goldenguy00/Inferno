using RoR2;
using RoR2.Achievements;

namespace Inferno.Unlocks
{
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
                var difficultyIndex = runReport.ruleBook.FindDifficulty();
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(difficultyIndex);
                if (difficultyDef != null && difficultyDef == Main.InfernoDiffDef || (difficultyIndex >= DifficultyIndex.Eclipse1 && Main.InfernalEclipse.Value))
                {
                    runReport.gameEnding.lunarCoinReward = 15;
                    runReport.gameEnding.showCredits = false;
                    Grant();
                }
            }
        }
    }

    [RegisterAchievement("CommandoClearGameInferno", "Skins.Inferno_Commando", null, 30, null)]
    public class CommandoClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CommandoBody");
        }
    }

    [RegisterAchievement("Bandit2ClearGameInferno", "Skins.Inferno_Bandit", "CompleteThreeStages", 30, null)]
    public class BanditClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("Bandit2Body");
        }
    }

    [RegisterAchievement("CaptainClearGameInferno", "Skins.Inferno_Captain", "CompleteMainEnding", 30, null)]
    public class CaptainClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CaptainBody");
        }
    }

    [RegisterAchievement("ArtificerClearGameInferno", "Skins.Inferno_Artificer", "FreeMage", 30, null)]
    public class ArtificerClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("MageBody");
        }
    }

    [RegisterAchievement("MercenaryClearGameInferno", "Skins.Inferno_Mercenary", "CompleteUnknownEnding", 30, null)]
    public class MercenaryClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("MercBody");
        }
    }

    [RegisterAchievement("RailgunnerClearGameInferno", "Skins.Inferno_Railgunner", null, 30, null)]
    public class RailgunnerClearGameInfernoAchievement : BasePerSurvivorClearGameInfernoAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RailgunnerBody");
        }
    }
}