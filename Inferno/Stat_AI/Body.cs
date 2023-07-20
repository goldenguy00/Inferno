using RoR2;

namespace Inferno.Stat_AI
{
    public static class Body
    {
        public static BodyIndex beetleQueenIndex;
        public static BodyIndex clayDunestriderIndex;
        public static BodyIndex grandparentIndex;
        public static BodyIndex grovetenderIndex;
        public static BodyIndex grovetenderWispIndex;
        public static BodyIndex impOverlordIndex;
        public static BodyIndex magmaWormIndex;
        public static BodyIndex mithrixIndex;
        public static BodyIndex mithrixP4Index;
        public static BodyIndex stoneTitanIndex;
        public static BodyIndex wanderingVagrantIndex;
        public static BodyIndex wanderingVagrantBombIndex;
        public static BodyIndex beetleIndex;
        public static BodyIndex beetleGuardIndex;
        public static BodyIndex bighornBisonIndex;
        public static BodyIndex blindPestIndex;
        public static BodyIndex clayTemplarIndex;
        public static BodyIndex elderLemurianIndex;
        public static BodyIndex greaterWispIndex;
        public static BodyIndex hermitCrabIndex;
        public static BodyIndex impIndex;
        public static BodyIndex jellyfishIndex;
        public static BodyIndex lemurianIndex;
        public static BodyIndex lesserWispIndex;
        public static BodyIndex lunarExploderIndex;
        public static BodyIndex lunarGolemIndex;
        public static BodyIndex lunarWispIndex;
        public static BodyIndex stoneGolemIndex;
        public static BodyIndex voidReaverIndex;
        public static BodyIndex xiConstructIndex;
        public static BodyIndex gupIndex;
        public static BodyIndex geepIndex;
        public static BodyIndex gipIndex;
        public static BodyIndex clayApothecaryIndex;
        public static BodyIndex parentIndex;
        public static BodyIndex miniMushrumIndex;
        public static BodyIndex solusControlUnitIndex;
        public static BodyIndex scavengerIndex;
        public static BodyIndex larvaIndex;
        public static BodyIndex aurelioniteIndex;
        public static BodyIndex solusProbeIndex;

        public static void Init()
        {
            On.RoR2.BodyCatalog.Init += BodyCatalog_Init;
        }

        private static void BodyCatalog_Init(On.RoR2.BodyCatalog.orig_Init orig)
        {
            orig();
            beetleQueenIndex = BodyCatalog.FindBodyIndex("BeetleQueen2Body");
            clayDunestriderIndex = BodyCatalog.FindBodyIndex("ClayBossBody");
            grandparentIndex = BodyCatalog.FindBodyIndex("GrandParentBody");
            grovetenderIndex = BodyCatalog.FindBodyIndex("GravekeeperBody");
            grovetenderWispIndex = BodyCatalog.FindBodyIndex("GravekeeperTrackingFireball");
            impOverlordIndex = BodyCatalog.FindBodyIndex("ImpBossBody");
            magmaWormIndex = BodyCatalog.FindBodyIndex("MagmaWormBody");
            mithrixIndex = BodyCatalog.FindBodyIndex("BrotherBody");
            mithrixP4Index = BodyCatalog.FindBodyIndex("BrotherHurtBody");
            stoneTitanIndex = BodyCatalog.FindBodyIndex("TitanBody");
            wanderingVagrantIndex = BodyCatalog.FindBodyIndex("VagrantBody");
            wanderingVagrantBombIndex = BodyCatalog.FindBodyIndex("VagrantTrackingBomb");
            beetleIndex = BodyCatalog.FindBodyIndex("BeetleBody");
            beetleGuardIndex = BodyCatalog.FindBodyIndex("BeetleGuardBody");
            bighornBisonIndex = BodyCatalog.FindBodyIndex("BisonBody");
            blindPestIndex = BodyCatalog.FindBodyIndex("FlyingVerminBody");
            clayTemplarIndex = BodyCatalog.FindBodyIndex("ClayBruiserBody");
            elderLemurianIndex = BodyCatalog.FindBodyIndex("LemurianBruiserBody");
            greaterWispIndex = BodyCatalog.FindBodyIndex("GreaterWispBody");
            hermitCrabIndex = BodyCatalog.FindBodyIndex("HermitCrabBody");
            impIndex = BodyCatalog.FindBodyIndex("ImpBody");
            jellyfishIndex = BodyCatalog.FindBodyIndex("JellyfishBody");
            lemurianIndex = BodyCatalog.FindBodyIndex("LemurianBody");
            lesserWispIndex = BodyCatalog.FindBodyIndex("WispBody");
            lunarExploderIndex = BodyCatalog.FindBodyIndex("LunarExploderBody");
            lunarGolemIndex = BodyCatalog.FindBodyIndex("LunarGolemBody");
            lunarWispIndex = BodyCatalog.FindBodyIndex("LunarWispBody");
            stoneGolemIndex = BodyCatalog.FindBodyIndex("GolemBody");
            voidReaverIndex = BodyCatalog.FindBodyIndex("NullifierBody");
            xiConstructIndex = BodyCatalog.FindBodyIndex("MegaConstructBody");
            gupIndex = BodyCatalog.FindBodyIndex("GupBody");
            geepIndex = BodyCatalog.FindBodyIndex("GeepBody");
            gipIndex = BodyCatalog.FindBodyIndex("GipBody");
            clayApothecaryIndex = BodyCatalog.FindBodyIndex("ClayGrenadierBody");
            parentIndex = BodyCatalog.FindBodyIndex("ParentBody");
            miniMushrumIndex = BodyCatalog.FindBodyIndex("MiniMushroomBody");
            solusControlUnitIndex = BodyCatalog.FindBodyIndex("RoboBallBossBody");
            scavengerIndex = BodyCatalog.FindBodyIndex("ScavBody");
            larvaIndex = BodyCatalog.FindBodyIndex("AcidLarvaBody");
            aurelioniteIndex = BodyCatalog.FindBodyIndex("TitanGoldBody");
            solusProbeIndex = BodyCatalog.FindBodyIndex("RoboBallMiniBody");
            /*
            Main.InfernoLogger.LogError(beetleQueenIndex);
            Main.InfernoLogger.LogError(clayDunestriderIndex);
            Main.InfernoLogger.LogError(grandparentIndex);
            Main.InfernoLogger.LogError(grovetenderIndex);
            Main.InfernoLogger.LogError(grovetenderWispIndex);
            Main.InfernoLogger.LogError(impOverlordIndex);
            Main.InfernoLogger.LogError(magmaWormIndex);
            Main.InfernoLogger.LogError(mithrixIndex);
            Main.InfernoLogger.LogError(mithrixP4Index);
            Main.InfernoLogger.LogError(stoneTitanIndex);
            Main.InfernoLogger.LogError(wanderingVagrantIndex);
            Main.InfernoLogger.LogError(wanderingVagrantBombIndex);
            Main.InfernoLogger.LogError(beetleIndex);
            Main.InfernoLogger.LogError(beetleGuardIndex);
            Main.InfernoLogger.LogError(bighornBisonIndex);
            Main.InfernoLogger.LogError(blindPestIndex);
            Main.InfernoLogger.LogError(clayTemplarIndex);
            Main.InfernoLogger.LogError(elderLemurianIndex);
            Main.InfernoLogger.LogError(greaterWispIndex);
            Main.InfernoLogger.LogError(hermitCrabIndex);
            Main.InfernoLogger.LogError(impIndex);
            Main.InfernoLogger.LogError(jellyfishIndex);
            Main.InfernoLogger.LogError(lemurianIndex);
            Main.InfernoLogger.LogError(lesserWispIndex);
            Main.InfernoLogger.LogError(lunarExploderIndex);
            Main.InfernoLogger.LogError(lunarGolemIndex);
            Main.InfernoLogger.LogError(lunarWispIndex);
            Main.InfernoLogger.LogError(stoneGolemIndex);
            Main.InfernoLogger.LogError(voidReaverIndex);
            Main.InfernoLogger.LogError(xiConstructIndex);
            Main.InfernoLogger.LogError(gupIndex);
            Main.InfernoLogger.LogError(geepIndex);
            Main.InfernoLogger.LogError(gipIndex);
            Main.InfernoLogger.LogError(clayApothecaryIndex);
            Main.InfernoLogger.LogError(parentIndex);
            Main.InfernoLogger.LogError(miniMushrumIndex);
            Main.InfernoLogger.LogError(solusControlUnitIndex);
            Main.InfernoLogger.LogError(scavengerIndex);
            Main.InfernoLogger.LogError(larvaIndex);
            Main.InfernoLogger.LogError(aurelioniteIndex);
            Main.InfernoLogger.LogError(solusProbeIndex);
            */

            // Everything is Fine!
        }

        public static void BodyChanges(CharacterBody body)
        {
            if (body.isPlayerControlled) return;
            // Main.InfernoLogger.LogError("body " + body.name + " is NOT player controlled");

            var setStateOnHurt = body.GetComponent<SetStateOnHurt>();
            var characterDirection = body.GetComponent<CharacterDirection>();
            var wormBodyPositions = body.GetComponent<WormBodyPositions2>();
            var wormBodyPositionsDriver = body.GetComponent<WormBodyPositionsDriver>();

            if (characterDirection) characterDirection.turnSpeed = 360f;
            if (setStateOnHurt) setStateOnHurt.canBeHitStunned = false;

            if (wormBodyPositions)
            {
                wormBodyPositions.speedMultiplier = 40f;
                wormBodyPositions.followDelay = 0.1f;
            }
            if (wormBodyPositionsDriver) wormBodyPositionsDriver.maxTurnSpeed = 1000f;

            switch (body.bodyIndex)
            {
                default:
                    break;

                case BodyIndex n when n == beetleQueenIndex:
                    body.baseMaxHealth = 2500f;
                    body.levelMaxHealth = 750f;
                    body.mainRootSpeed = 14f;
                    body.baseMoveSpeed = 14f;
                    body.rootMotionInMainState = false;
                    characterDirection.driveFromRootRotation = false;
                    characterDirection.turnSpeed = 100f;
                    break;

                case BodyIndex n when n == clayDunestriderIndex:
                    body.baseMaxHealth = 2500f;
                    body.levelMaxHealth = 750f;
                    body.baseMoveSpeed = 14f;
                    break;

                case BodyIndex n when n == grandparentIndex:
                    body.baseMaxHealth = 4000f;
                    body.levelMaxHealth = 1200f;
                    break;

                case BodyIndex n when n == grovetenderIndex:
                    body.baseMaxHealth = 3200f;
                    body.levelMaxHealth = 960f;
                    body.baseMoveSpeed = 24f;
                    break;

                case BodyIndex n when n == grovetenderWispIndex:
                    body.baseMoveSpeed = 50f;
                    body.baseMaxHealth = 50f;
                    body.levelMaxHealth = 15f;
                    break;

                case BodyIndex n when n == impOverlordIndex:
                    body.baseMaxHealth = 3200f;
                    body.levelMaxHealth = 960f;
                    body.baseMoveSpeed = 13f;
                    body.baseAcceleration = 200f;
                    break;

                case BodyIndex n when n == magmaWormIndex:
                    body.baseMaxHealth = 2500f;
                    body.levelMaxHealth = 750f;
                    break;

                case BodyIndex n when n == mithrixIndex:
                    if (Main.EnableMithrixChanges.Value)
                    {
                        body.baseAcceleration = 200f;
                        body.baseMoveSpeed = 17f;
                        body.baseMaxHealth = 800f;
                        body.levelMaxHealth = 240f;
                        body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                        body.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
                    }
                    break;

                case BodyIndex n when n == mithrixP4Index:
                    if (Main.EnableMithrixChanges.Value)
                    {
                        body.baseMaxHealth = 500f;
                        body.levelMaxHealth = 150f;
                        body.baseMoveSpeed = 10f;
                        body.sprintingSpeedMultiplier = 1.45f;
                        body.baseDamage = 5f;
                        body.levelDamage = 1f;
                        body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                        body.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
                    }
                    break;

                case BodyIndex n when n == stoneTitanIndex:
                    body.baseMaxHealth = 2500f;
                    body.levelMaxHealth = 750f;
                    body.baseMoveSpeed = 12f;
                    break;

                case BodyIndex n when n == wanderingVagrantIndex:
                    body.baseMaxHealth = 2500f;
                    body.levelMaxHealth = 750f;
                    body.baseMoveSpeed = 14f;
                    body.baseAcceleration = 500f;
                    break;

                case BodyIndex n when n == wanderingVagrantBombIndex:
                    body.baseMaxHealth = 90f;
                    body.levelMaxHealth = 27f;
                    break;

                case BodyIndex n when n == beetleIndex:
                    body.baseMoveSpeed = 12f;
                    setStateOnHurt.canBeStunned = false;
                    body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                    break;

                case BodyIndex n when n == beetleGuardIndex:
                    body.baseMoveSpeed = 22f;
                    break;

                case BodyIndex n when n == bighornBisonIndex:
                    body.baseMoveSpeed = 5f;
                    body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                    break;

                case BodyIndex n when n == blindPestIndex:
                    body.baseMoveSpeed = 9f;
                    body.baseDamage = 10f;
                    body.levelDamage = 2f;
                    break;

                case BodyIndex n when n == clayTemplarIndex:
                    body.baseMoveSpeed = 11f;
                    body.baseDamage = 12f;
                    body.levelDamage = 2.4f;
                    break;

                case BodyIndex n when n == elderLemurianIndex:
                    body.baseMoveSpeed = 16f;
                    break;

                case BodyIndex n when n == greaterWispIndex:
                    body.baseMoveSpeed = 12f;
                    break;

                case BodyIndex n when n == hermitCrabIndex:
                    body.baseMoveSpeed = 22f;
                    break;

                case BodyIndex n when n == impIndex:
                    body.baseMoveSpeed = 15f;
                    break;

                case BodyIndex n when n == jellyfishIndex:
                    body.baseMoveSpeed = 13f;
                    break;

                case BodyIndex n when n == lemurianIndex:
                    body.baseMoveSpeed = 10f;
                    break;

                case BodyIndex n when n == lesserWispIndex:
                    body.baseMoveSpeed = 12f;
                    body.baseAcceleration = 24f;
                    setStateOnHurt.canBeStunned = false;
                    break;

                case BodyIndex n when n == lunarExploderIndex:
                    body.baseMoveSpeed = 16f;
                    body.baseAcceleration = 1000f;
                    break;

                case BodyIndex n when n == lunarGolemIndex:
                    body.baseMoveSpeed = 16f;
                    break;

                case BodyIndex n when n == lunarWispIndex:
                    body.baseDamage = 10f;
                    break;

                case BodyIndex n when n == stoneGolemIndex:
                    body.baseDamage = 16f;
                    body.levelDamage = 3.2f;
                    body.baseMoveSpeed = 8f;
                    break;

                case BodyIndex n when n == voidReaverIndex:
                    body.baseMoveSpeed = 17f;
                    setStateOnHurt.canBeStunned = false;
                    break;

                case BodyIndex n when n == xiConstructIndex:
                    body.baseMoveSpeed = 35f;
                    break;

                case BodyIndex n when n == gupIndex:
                    body.baseMoveSpeed = 14f;
                    body.baseMaxHealth = 700f;
                    body.levelMaxHealth = 210f;
                    break;

                case BodyIndex n when n == geepIndex:
                    body.baseMoveSpeed = 19f;
                    body.baseMaxHealth = 550f;
                    body.levelMaxHealth = 165f;
                    body.baseDamage = 11f;
                    body.levelDamage = 2.2f;
                    break;

                case BodyIndex n when n == gipIndex:
                    body.baseMoveSpeed = 24f;
                    body.baseMaxHealth = 350f;
                    body.levelMaxHealth = 105f;
                    body.baseDamage = 10f;
                    body.levelDamage = 2f;
                    break;

                case BodyIndex n when n == clayApothecaryIndex:
                    body.baseMoveSpeed = 12f;
                    break;

                case BodyIndex n when n == parentIndex:
                    body.baseMoveSpeed = 17f;
                    break;

                case BodyIndex n when n == miniMushrumIndex:
                    body.baseMoveSpeed = 6f;
                    break;

                case BodyIndex n when n == solusControlUnitIndex:
                    body.baseMoveSpeed = 14f;
                    body.baseMaxHealth = 3000f;
                    body.levelMaxHealth = 900f;
                    break;

                case BodyIndex n when n == scavengerIndex:
                    body.baseMoveSpeed = 9f;
                    break;

                case BodyIndex n when n == larvaIndex:
                    body.sprintingSpeedMultiplier = 3f;
                    body.baseMaxHealth = 80f;
                    body.levelMaxHealth = 24f;
                    break;

                case BodyIndex n when n == aurelioniteIndex:
                    body.baseMaxHealth = 2500f;
                    body.levelMaxHealth = 750f;
                    body.baseMoveSpeed = 12f;
                    break;

                case BodyIndex n when n == solusProbeIndex:
                    body.baseMoveSpeed = 13f;
                    break;
            }
        }
    }
}