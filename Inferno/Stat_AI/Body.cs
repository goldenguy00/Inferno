using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inferno.Stat_AI
{
    public static class Body
    {
        public static void BodyChanges(CharacterBody body)
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
                    if (Main.EnableMithrixChanges.Value)
                    {
                        cb.baseAcceleration = 200f;
                        cb.baseMoveSpeed = 17f;
                        cb.baseMaxHealth = 800f;
                        cb.levelMaxHealth = 240f;
                        cb.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                        cb.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
                    }
                    break;

                case "BrotherHurtBody(Clone)":
                    if (Main.EnableMithrixChanges.Value)
                    {
                        cb.baseMaxHealth = 500f;
                        cb.levelMaxHealth = 150f;
                        cb.baseMoveSpeed = 10f;
                        cb.sprintingSpeedMultiplier = 1.45f;
                        cb.baseDamage = 5f;
                        cb.levelDamage = 1f;
                        cb.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                        cb.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
                    }
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
                    cb.baseDamage = 12f;
                    cb.levelDamage = 2.4f;
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
                    cb.baseDamage = 10f;
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
                    cb.baseMaxHealth = 80f;
                    cb.levelMaxHealth = 24f;
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
    }
}