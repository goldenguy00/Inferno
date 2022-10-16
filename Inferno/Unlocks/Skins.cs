using RoR2;
using RoR2.Achievements;
using System;
using System.Collections.Generic;
using System.Text;

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
}
