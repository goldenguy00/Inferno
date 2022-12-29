using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;
using RoR2.Projectile;
using Random = UnityEngine.Random;
using EntityStates.BrotherMonster.Weapon;
using UnityEngine.AddressableAssets;

namespace Inferno.Skill_Misc
{
    public static class Hooks
    {
        public static void SpeedUpProjectiles2(On.RoR2.Projectile.ProjectileController.orig_Start orig, ProjectileController self)
        {
            orig(self);
            if (self.gameObject != null && self.teamFilter.teamIndex != TeamIndex.Player && self.gameObject.GetComponent<ProjectileCharacterController>() != null)
            {
                var pcc = self.gameObject.GetComponent<ProjectileCharacterController>();
                pcc.velocity *= Main.ProjectileSpeed.Value;
                if (pcc.lifetime < 4f)
                {
                    pcc.lifetime = 4f;
                }
            }
        }

        public static void SlideIntroState(On.EntityStates.BrotherMonster.SlideIntroState.orig_OnEnter orig, EntityStates.BrotherMonster.SlideIntroState self)
        {
            if (self.isAuthority)
            {
                Ray aimRay = self.GetAimRay();
                for (int i = 0; i < 6; i++)
                {
                    ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.Weapon.FireLunarShards.projectilePrefab, aimRay.origin, Quaternion.LookRotation(aimRay.direction), self.gameObject, self.characterBody.damage * 0.03f / 12f, 0f, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), DamageColorIndex.Default, null, -1f);
                    aimRay.direction = Util.ApplySpread(aimRay.direction, 0f, 4f, 4f, 4f, 0f, 0f);
                }
            }
            orig(self);
        }

        public static void FireWave(On.EntityStates.BrotherMonster.UltChannelState.orig_FireWave orig, EntityStates.BrotherMonster.UltChannelState self)
        {
            if (self.isAuthority)
            {
                float waves = 4f;
                float SpinnyCount = 360f / waves;
                Vector3 plane = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                Transform transgender = self.FindModelChild(EntityStates.BrotherMonster.WeaponSlam.muzzleString);
                Vector3 pos = transgender.position + new Vector3(Random.Range(-50f, 50f), 0f, Random.Range(-50f, 50f));
                int l = 0;
                while (l < waves)
                {
                    Vector3 zase = Quaternion.AngleAxis(SpinnyCount * l, Vector3.up) * plane;
                    ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.UltChannelState.waveProjectileLeftPrefab, pos, Util.QuaternionSafeLookRotation(zase), self.gameObject, self.characterBody.damage * 4f, EntityStates.BrotherMonster.FistSlam.waveProjectileForce, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), 0, null, -1f);
                    l++;
                }
            }
            orig(self);
        }

        public static void HealthComponent_Awake(On.RoR2.HealthComponent.orig_Awake orig, HealthComponent self)
        {
            self.gameObject.AddComponent<InfernoPermanentDamage>();
            orig(self);
        }

        public static void SpawnState(On.EntityStates.VagrantMonster.SpawnState.orig_OnEnter orig, EntityStates.VagrantMonster.SpawnState self)
        {
            EntityStates.VagrantMonster.SpawnState.duration = 2f;
            orig(self);
        }

        public static void FaceSlam(On.EntityStates.ClayGrenadier.FaceSlam.orig_OnEnter orig, EntityStates.ClayGrenadier.FaceSlam self)
        {
            EntityStates.ClayGrenadier.FaceSlam.blastRadius = 10f;
            orig(self);
        }

        public static void FireEmbers(On.EntityStates.Wisp1Monster.FireEmbers.orig_OnEnter orig, EntityStates.Wisp1Monster.FireEmbers self)
        {
            EntityStates.Wisp1Monster.FireEmbers.damageCoefficient = 1.6f;
            orig(self);
        }

        public static void Phase1(On.EntityStates.Missions.BrotherEncounter.Phase1.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase1 self)
        {
            Main.Ramp1.SetActive(false);
            Main.Ramp2.SetActive(false);
            Main.Ramp3.SetActive(false);
            Main.Rocks.SetActive(false);
            orig(self);
        }

        public static void CacheObjects(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            if (SceneManager.GetActiveScene().name == "moon2")
            {
                var Arena = GameObject.Find("HOLDER: Final Arena").transform;
                Main.Ramp1 = Arena.GetChild(0).gameObject;
                Main.Ramp2 = Arena.GetChild(1).gameObject;
                Main.Ramp3 = Arena.GetChild(2).gameObject;
                Main.Rocks = Arena.GetChild(6).gameObject;
            }
            orig(self);
        }

        public static void Phase4(On.EntityStates.Missions.BrotherEncounter.Phase4.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase4 self)
        {
            Main.Ramp1.SetActive(false);
            Main.Ramp2.SetActive(false);
            Main.Ramp3.SetActive(false);
            Main.Rocks.SetActive(false);
            orig(self);
        }

        public static void CombatDirector_Awake(On.RoR2.CombatDirector.orig_Awake orig, CombatDirector self)
        {
            if (Main.EnableCDirector.Value && !RiskyModCompat.enabled)
            {
                self.maxConsecutiveCheapSkips = 2;
                self.eliteBias = 0.9f;
                self.maxSpawnDistance = 80f;
                self.minSpawnRange = 14f;
                self.creditMultiplier = 1f + (Main.CreditMultiplier.Value * Run.instance.stageClearCount);
                self.goldRewardCoefficient = 0.94f / self.creditMultiplier;
            }
            orig(self);
        }

        public static CharacterMaster IncreaseMonsterCap(On.RoR2.MasterSummon.orig_Perform orig, MasterSummon self)
        {
            TeamIndex teamIndex = TeamIndex.Monster;
            TeamDef teamDef = TeamCatalog.GetTeamDef(teamIndex);
            if (teamDef != null)
            {
                teamDef.softCharacterLimit = Main.MonsterLimit.Value;
            }
            return orig(self);
        }

        public static void CleanupDeathState(On.EntityStates.NullifierMonster.DeathState.orig_OnEnter orig, EntityStates.NullifierMonster.DeathState self)
        {
            EntityStates.NullifierMonster.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 3f;
            orig(self);
        }

        public static void CleanupDeathState2(On.EntityStates.VoidMegaCrab.DeathState.orig_OnEnter orig, EntityStates.VoidMegaCrab.DeathState self)
        {
            EntityStates.VoidMegaCrab.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 5f;
            orig(self);
        }

        public static void DeathState(On.EntityStates.NullifierMonster.DeathState.orig_OnEnter orig, EntityStates.NullifierMonster.DeathState self)
        {
            EntityStates.NullifierMonster.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 2.6f / self.attackSpeedStat;
            orig(self);
        }

        public static void DeathState2(On.EntityStates.VoidMegaCrab.DeathState.orig_OnEnter orig, EntityStates.VoidMegaCrab.DeathState self)
        {
            EntityStates.VoidMegaCrab.DeathState.deathBombProjectile.GetComponent<ProjectileImpactExplosion>().lifetime = 3.8f / self.attackSpeedStat;
            orig(self);
        }

        public static void FireHook(On.EntityStates.GravekeeperBoss.FireHook.orig_OnEnter orig, EntityStates.GravekeeperBoss.FireHook self)
        {
            EntityStates.GravekeeperBoss.FireHook.projectilePrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/GravekeeperHookProjectile");
            EntityStates.GravekeeperBoss.FireHook.projectileDamageCoefficient = 1.3f;
            EntityStates.GravekeeperBoss.FireHook.baseDuration = 2.1f;
            EntityStates.GravekeeperBoss.FireHook.projectileCount = 40;
            EntityStates.GravekeeperBoss.FireHook.spread = 360f;
            orig(self);
        }

        public static void SpawnWards(On.EntityStates.BeetleQueenMonster.SpawnWards.orig_OnEnter orig, EntityStates.BeetleQueenMonster.SpawnWards self)
        {
            EntityStates.BeetleQueenMonster.SpawnWards.baseDuration = 3.5f;
            EntityStates.BeetleQueenMonster.SpawnWards.orbCountMax = 10;
            EntityStates.BeetleQueenMonster.SpawnWards.orbRange = 100f;
            orig(self);
        }

        public static void FireSpit(On.EntityStates.BeetleQueenMonster.FireSpit.orig_OnEnter orig, EntityStates.BeetleQueenMonster.FireSpit self)
        {
            EntityStates.BeetleQueenMonster.FireSpit.baseDuration = 1.5f;
            orig(self);
        }

        public static void PrepHook(On.EntityStates.GravekeeperBoss.PrepHook.orig_OnEnter orig, EntityStates.GravekeeperBoss.PrepHook self)
        {
            EntityStates.GravekeeperBoss.PrepHook.baseDuration = 1.3f;
            orig(self);
        }

        public static void GravekeeperBarrage(On.EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.orig_OnEnter orig, EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage self)
        {
            EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.damageCoefficient = 2f;
            EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.missileSpawnDelay = 0.15f;
            EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.missileSpawnFrequency = 5f;
            orig(self);
        }

        public static void HoldSkyLeap(On.EntityStates.BrotherMonster.HoldSkyLeap.orig_OnEnter orig, EntityStates.BrotherMonster.HoldSkyLeap self)
        {
            if (Main.MithrixAS.Value)
            {
                EntityStates.BrotherMonster.HoldSkyLeap.duration = 2f / self.attackSpeedStat;
            }
            else
            {
                EntityStates.BrotherMonster.HoldSkyLeap.duration = 2f;
            }
            if (NetworkServer.active)
            {
                Util.CleanseBody(self.characterBody, true, false, false, true, true, false);
            }
            orig(self);
        }

        public static void ExitSkyLeap(On.EntityStates.BrotherMonster.ExitSkyLeap.orig_OnEnter orig, EntityStates.BrotherMonster.ExitSkyLeap self)
        {
            EntityStates.BrotherMonster.ExitSkyLeap.waveProjectileCount = 20;
            EntityStates.BrotherMonster.ExitSkyLeap.waveProjectileDamageCoefficient = 2f;
            orig(self);
        }

        public static void WeaponSlam(On.EntityStates.BrotherMonster.WeaponSlam.orig_OnEnter orig, EntityStates.BrotherMonster.WeaponSlam self)
        {
            if (Main.MithrixAS.Value)
            {
                EntityStates.BrotherMonster.WeaponSlam.duration = 3f / self.attackSpeedStat;
            }
            else
            {
                EntityStates.BrotherMonster.WeaponSlam.duration = 3f;
            }
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileArc = 360f;
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileCount = 8;
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileDamageCoefficient = 2f;
            EntityStates.BrotherMonster.WeaponSlam.waveProjectileForce = -1600f;
            EntityStates.BrotherMonster.WeaponSlam.weaponForce = -2300f;
            var pillarprefab = EntityStates.BrotherMonster.WeaponSlam.pillarProjectilePrefab;
            pillarprefab.transform.localScale = new Vector3(4f, 4f, 4f);
            var ghost = pillarprefab.GetComponent<ProjectileController>().ghostPrefab;
            ghost.transform.localScale = new Vector3(4f, 4f, 4f);
            Main.hasFired = false;
            orig(self);
        }

        public static void BaseSlideState(On.EntityStates.BrotherMonster.BaseSlideState.orig_OnEnter orig, EntityStates.BrotherMonster.BaseSlideState self)
        {
            if (Main.MithrixAS.Value)
            {
                EntityStates.BrotherMonster.BaseSlideState.duration /= self.attackSpeedStat;
            }
            switch (self)
            {
                case EntityStates.BrotherMonster.SlideBackwardState:
                    self.slideRotation = Quaternion.identity;
                    break;

                case EntityStates.BrotherMonster.SlideLeftState:
                    self.slideRotation = Quaternion.AngleAxis(-40f, Vector3.up);
                    break;

                case EntityStates.BrotherMonster.SlideRightState:
                    self.slideRotation = Quaternion.AngleAxis(40f, Vector3.up);
                    break;
            }
            orig(self);
        }

        public static void SprintBash(On.EntityStates.BrotherMonster.SprintBash.orig_OnEnter orig, EntityStates.BrotherMonster.SprintBash self)
        {
            EntityStates.BrotherMonster.SprintBash.durationBeforePriorityReduces = 0.18f;
            if (Main.MithrixAS.Value)
            {
                self.baseDuration = 1.4f / self.attackSpeedStat;
            }
            else
            {
                self.baseDuration = 1.4f;
            }
            self.damageCoefficient = 1.5f;
            self.pushAwayForce = 1500f;
            self.forceVector = new Vector3(0f, 750f, 0f);
            if (self.isAuthority)
            {
                for (int i = 0; i < 6; i++)
                {
                    Ray aimRay = self.GetAimRay();
                    Vector3 vector = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 0f, (float)i * 5f, 0f);
                    ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.Weapon.FireLunarShards.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(vector), self.gameObject, self.characterBody.damage * 0.05f / 12f, 0f, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), DamageColorIndex.Default, null, -1f);
                    Ray aimRay2 = self.GetAimRay();
                    Vector3 vector2 = Util.ApplySpread(aimRay2.direction, 0f, 0f, 1f, 0f, -(float)i * 5f, 0f);
                    ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.Weapon.FireLunarShards.projectilePrefab, aimRay2.origin, Util.QuaternionSafeLookRotation(vector2), self.gameObject, self.characterBody.damage * 0.05f / 12f, 0f, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), DamageColorIndex.Default, null, -1f);
                }
            }
            orig(self);
        }

        public static void FireLunarShards(On.EntityStates.BrotherMonster.Weapon.FireLunarShards.orig_OnEnter orig, EntityStates.BrotherMonster.Weapon.FireLunarShards self)
        {
            EntityStates.BrotherMonster.Weapon.FireLunarShards.spreadBloomValue = 20f;
            EntityStates.BrotherMonster.Weapon.FireLunarShards.recoilAmplitude = 2f;
            EntityStates.BrotherMonster.Weapon.FireLunarShards.baseDuration = 0.03f;
            Main.ShardCount++;
            if (self is FireLunarShardsHurt)
            {
                if (self.isAuthority && Main.ShardCount == 9)
                {
                    float waves = 3f;
                    float SpinnyCount = 360f / waves;
                    Vector3 plane = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                    Transform transgender = self.FindModelChild(EntityStates.BrotherMonster.WeaponSlam.muzzleString);
                    Vector3 pos = transgender.position + new Vector3(Random.Range(-50f, 50f), 0f, Random.Range(-50f, 50f));
                    int l = 0;
                    while (l < waves)
                    {
                        Vector3 zase = Quaternion.AngleAxis(SpinnyCount * l, Vector3.up) * plane;
                        ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.UltChannelState.waveProjectileLeftPrefab, pos, Util.QuaternionSafeLookRotation(zase), self.gameObject, self.characterBody.damage * 3.5f, EntityStates.BrotherMonster.FistSlam.waveProjectileForce, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), 0, null, -1f);
                        l++;
                        Main.ShardCount = 0;
                    }
                }
            }
            else
            {
                Main.ShardCount = 0;
            }
            orig(self);
        }

        public static void Phase2(On.EntityStates.Missions.BrotherEncounter.Phase2.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase2 self)
        {
            orig(self);
            self.PreEncounterBegin();
            self.outer.SetNextState(new EntityStates.Missions.BrotherEncounter.Phase3());
        }

        public static void FistSlam(On.EntityStates.BrotherMonster.FistSlam.orig_OnEnter orig, EntityStates.BrotherMonster.FistSlam self)
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
                    ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.UltChannelState.waveProjectileLeftPrefab, pos, Util.QuaternionSafeLookRotation(zase), self.gameObject, self.characterBody.damage * 3.5f, EntityStates.BrotherMonster.FistSlam.waveProjectileForce, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), 0, null, -1f);
                    l++;
                }
            }
            orig(self);
        }

        public static void SpellChannelEnterState(On.EntityStates.BrotherMonster.SpellChannelEnterState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelEnterState self)
        {
            EntityStates.BrotherMonster.SpellChannelEnterState.duration = 3f;
            orig(self);
        }

        public static void SpellChannelState(On.EntityStates.BrotherMonster.SpellChannelState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelState self)
        {
            EntityStates.BrotherMonster.SpellChannelState.stealInterval = 0.05f;
            EntityStates.BrotherMonster.SpellChannelState.delayBeforeBeginningSteal = 0f;
            EntityStates.BrotherMonster.SpellChannelState.maxDuration = 15f;
            orig(self);
        }

        public static void SpellChannelExitState(On.EntityStates.BrotherMonster.SpellChannelExitState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelExitState self)
        {
            EntityStates.BrotherMonster.SpellChannelExitState.lendInterval = 0.04f;
            EntityStates.BrotherMonster.SpellChannelExitState.duration = 2.5f;
            orig(self);
        }

        public static void StaggerEnter(On.EntityStates.BrotherMonster.StaggerEnter.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerEnter self)
        {
            self.duration = 0f;
            orig(self);
        }

        public static void StaggerExit(On.EntityStates.BrotherMonster.StaggerExit.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerExit self)
        {
            self.duration = 0f;
            orig(self);
        }

        public static void StaggerLoop(On.EntityStates.BrotherMonster.StaggerLoop.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerLoop self)
        {
            self.duration = 0f;
            orig(self);
        }

        public static void TrueDeathState(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self)
        {
            EntityStates.BrotherMonster.TrueDeathState.dissolveDuration = 5f;
            orig(self);
        }

        public static void FireRandomProjectiles(On.EntityStates.BrotherHaunt.FireRandomProjectiles.orig_OnEnter orig, EntityStates.BrotherHaunt.FireRandomProjectiles self)
        {
            EntityStates.BrotherHaunt.FireRandomProjectiles.maximumCharges = 150;
            EntityStates.BrotherHaunt.FireRandomProjectiles.chargeRechargeDuration = 0.03f;
            EntityStates.BrotherHaunt.FireRandomProjectiles.chanceToFirePerSecond = 0.5f;
            EntityStates.BrotherHaunt.FireRandomProjectiles.damageCoefficient = 12f;
            orig(self);
        }

        public static void FireFist(On.EntityStates.TitanMonster.FireFist.orig_OnEnter orig, EntityStates.TitanMonster.FireFist self)
        {
            EntityStates.TitanMonster.FireFist.entryDuration = 1.6f;
            EntityStates.TitanMonster.FireFist.fireDuration = 1.1f;
            EntityStates.TitanMonster.FireFist.exitDuration = 1.6f;
            EntityStates.TitanMonster.FireFist.fistDamageCoefficient = 1.2f;
            orig(self);
        }

        public static void HeadbuttState(On.EntityStates.BeetleMonster.HeadbuttState.orig_FixedUpdate orig, EntityStates.BeetleMonster.HeadbuttState self)
        {
            EntityStates.BeetleMonster.HeadbuttState.baseDuration = 2.7f;
            if (self.isAuthority)
            {
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
            }
            orig(self);

            // results in double hits sometimes, no idea how to fix it!
        }

        public static void GenericCharacterSpawnState(On.EntityStates.GenericCharacterSpawnState.orig_OnEnter orig, EntityStates.GenericCharacterSpawnState self)
        {
            orig(self);
            if (self.duration > 2.5f)
            {
                self.duration = 2.5f;
            }
        }

        public static void FireSunder(On.EntityStates.BeetleGuardMonster.FireSunder.orig_OnEnter orig, EntityStates.BeetleGuardMonster.FireSunder self)
        {
            EntityStates.BeetleGuardMonster.FireSunder.baseDuration = 1.35f;
            orig(self);
        }

        public static void GroundSlam(On.EntityStates.BeetleGuardMonster.GroundSlam.orig_OnEnter orig, EntityStates.BeetleGuardMonster.GroundSlam self)
        {
            EntityStates.BeetleGuardMonster.GroundSlam.baseDuration = 2.2f;
            orig(self);
        }

        public static void Headbutt(On.EntityStates.Bison.Headbutt.orig_OnEnter orig, EntityStates.Bison.Headbutt self)
        {
            EntityStates.Bison.Headbutt.baseHeadbuttDuration = 2.5f;
            orig(self);
        }

        public static void Charge(On.EntityStates.Bison.Charge.orig_OnEnter orig, EntityStates.Bison.Charge self)
        {
            EntityStates.Bison.Charge.chargeMovementSpeedCoefficient = 12f;
            EntityStates.Bison.Charge.turnSpeed = 99999f;
            EntityStates.Bison.Charge.turnSmoothTime = 0f;
            EntityStates.Bison.Charge.selfStunDuration = 0.5f;
            orig(self);
        }

        public static void FireSonicBoom(On.EntityStates.ClayBruiser.Weapon.FireSonicBoom.orig_OnEnter orig, EntityStates.ClayBruiser.Weapon.FireSonicBoom self)
        {
            self.baseDuration = 0.8f;
            self.fieldOfView = 90f;
            orig(self);
        }

        public static void FireMegaFireball(On.EntityStates.LemurianBruiserMonster.FireMegaFireball.orig_OnEnter orig, EntityStates.LemurianBruiserMonster.FireMegaFireball self)
        {
            EntityStates.LemurianBruiserMonster.FireMegaFireball.projectileCount = 10;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.totalYawSpread = 90f;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.baseFireDuration = 0.5f;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.projectileSpeed = 50f;
            EntityStates.LemurianBruiserMonster.FireMegaFireball.damageCoefficient = 1.75f;
            orig(self);
        }

        public static void ChargeCannons(On.EntityStates.GreaterWispMonster.ChargeCannons.orig_OnEnter orig, EntityStates.GreaterWispMonster.ChargeCannons self)
        {
            self.baseDuration = 1.25f;
            orig(self);
        }

        public static void Bite1(On.EntityStates.LemurianMonster.Bite.orig_OnEnter orig, EntityStates.LemurianMonster.Bite self)
        {
            EntityStates.LemurianMonster.Bite.radius = 3f;
            EntityStates.LemurianMonster.Bite.baseDuration = 0.8f;
            EntityStates.LemurianMonster.Bite.forceMagnitude = 400f;
            orig(self);
        }

        public static void Bite2(On.EntityStates.LemurianMonster.Bite.orig_FixedUpdate orig, EntityStates.LemurianMonster.Bite self)
        {
            if (self.isAuthority)
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
            }
            orig(self);
        }

        public static void ChargeFireball(On.EntityStates.LemurianMonster.ChargeFireball.orig_OnEnter orig, EntityStates.LemurianMonster.ChargeFireball self)
        {
            EntityStates.LemurianMonster.ChargeFireball.baseDuration = 0.48f;
            orig(self);
        }

        public static void ClapState(On.EntityStates.GolemMonster.ClapState.orig_OnEnter orig, EntityStates.GolemMonster.ClapState self)
        {
            EntityStates.GolemMonster.ClapState.damageCoefficient = 3.5f;
            EntityStates.GolemMonster.ClapState.radius = 6f;
            EntityStates.GolemMonster.ClapState.duration = 1.6f;
            orig(self);
        }

        public static void ChargeLaser(On.EntityStates.GolemMonster.ChargeLaser.orig_OnEnter orig, EntityStates.GolemMonster.ChargeLaser self)
        {
            EntityStates.GolemMonster.ChargeLaser.baseDuration = 1.75f;
            orig(self);
        }

        public static void FireLaser(On.EntityStates.GolemMonster.FireLaser.orig_OnEnter orig, EntityStates.GolemMonster.FireLaser self)
        {
            EntityStates.GolemMonster.FireLaser.damageCoefficient = 2.5f;
            EntityStates.GolemMonster.FireLaser.blastRadius = 6f;
            orig(self);
        }

        public static void FireFireball(On.EntityStates.LemurianMonster.FireFireball.orig_OnEnter orig, EntityStates.LemurianMonster.FireFireball self)
        {
            EntityStates.LemurianMonster.FireFireball.damageCoefficient = 1.2f;
            orig(self);
        }

        public static void SporeGrenade(On.EntityStates.MiniMushroom.SporeGrenade.orig_OnEnter orig, EntityStates.MiniMushroom.SporeGrenade self)
        {
            EntityStates.MiniMushroom.SporeGrenade.baseChargeTime = 1.25f;
            EntityStates.MiniMushroom.SporeGrenade.baseDuration = 1.25f;
            EntityStates.MiniMushroom.SporeGrenade.damageCoefficient = 1.1f;
            orig(self);
        }

        public static void ChargeFire(On.EntityStates.VoidBarnacle.Weapon.ChargeFire.orig_OnEnter orig, EntityStates.VoidBarnacle.Weapon.ChargeFire self)
        {
            self.baseDuration = 0.6f;
            orig(self);
        }

        public static void Fire(On.EntityStates.VoidBarnacle.Weapon.Fire.orig_OnEnter orig, EntityStates.VoidBarnacle.Weapon.Fire self)
        {
            self.numberOfFireballs = 2;
            orig(self);
        }

        public static void PrepTarBall(On.EntityStates.ClayBoss.PrepTarBall.orig_OnEnter orig, EntityStates.ClayBoss.PrepTarBall self)
        {
            EntityStates.ClayBoss.PrepTarBall.baseDuration = 1.9f;
            orig(self);
        }

        public static void FireTarBall(On.EntityStates.ClayBoss.FireTarball.orig_OnEnter orig, EntityStates.ClayBoss.FireTarball self)
        {
            EntityStates.ClayBoss.FireTarball.tarballCountMax = 5;
            EntityStates.ClayBoss.FireTarball.baseTimeBetweenShots = 0.7f;
            EntityStates.ClayBoss.FireTarball.cooldownDuration = 1.8f;
            orig(self);
        }

        public static void GupSpikesState(On.EntityStates.BasicMeleeAttack.orig_OnEnter orig, EntityStates.BasicMeleeAttack self)
        {
            if (self is EntityStates.Gup.GupSpikesState)
            {
                self.damageCoefficient = 2.8f;
            }
            orig(self);
        }

        public static void FireLunarGuns(On.EntityStates.LunarWisp.FireLunarGuns.orig_OnEnter orig, EntityStates.LunarWisp.FireLunarGuns self)
        {
            EntityStates.LunarWisp.FireLunarGuns.baseDamagePerSecondCoefficient = 1.8f;
            orig(self);
        }

        public static void FirePortalBomb(On.EntityStates.NullifierMonster.FirePortalBomb.orig_OnEnter orig, EntityStates.NullifierMonster.FirePortalBomb self)
        {
            EntityStates.NullifierMonster.FirePortalBomb.baseDuration = 0.25f;
            EntityStates.NullifierMonster.FirePortalBomb.damageCoefficient = 1.8f;
            EntityStates.NullifierMonster.FirePortalBomb.portalBombCount = 5;
            orig(self);
        }

        public static void DoubleSlash(On.EntityStates.ImpMonster.DoubleSlash.orig_OnEnter orig, EntityStates.ImpMonster.DoubleSlash self)
        {
            EntityStates.ImpMonster.DoubleSlash.selfForce = 2000f;
            EntityStates.ImpMonster.DoubleSlash.walkSpeedPenaltyCoefficient = 1.2f;
            EntityStates.ImpMonster.DoubleSlash.damageCoefficient = 2.1f;
            orig(self);
        }

        public static void ChargeConstructBeam(On.EntityStates.MinorConstruct.Weapon.ChargeConstructBeam.orig_OnEnter orig, EntityStates.MinorConstruct.Weapon.ChargeConstructBeam self)
        {
            self.baseDuration = 0.33f;
            orig(self);
        }

        public static void FireConstructBeam(On.EntityStates.MinorConstruct.Weapon.FireConstructBeam.orig_OnEnter orig, EntityStates.MinorConstruct.Weapon.FireConstructBeam self)
        {
            self.baseDelayBeforeFiringProjectile = 0f;
            orig(self);
        }

        public static void FireLaser(On.EntityStates.MajorConstruct.Weapon.FireLaser.orig_OnEnter orig, EntityStates.MajorConstruct.Weapon.FireLaser self)
        {
            self.aimMaxSpeed = 17f;
            orig(self);
        }

        public static void BlinkState(On.EntityStates.ImpMonster.BlinkState.orig_OnEnter orig, EntityStates.ImpMonster.BlinkState self)
        {
            EntityStates.ImpMonster.BlinkState.blinkDistance = 18f;
            orig(self);
        }

        public static void FireMortar(On.EntityStates.HermitCrab.FireMortar.orig_OnEnter orig, EntityStates.HermitCrab.FireMortar self)
        {
            EntityStates.HermitCrab.FireMortar.mortarCount = 2;
            EntityStates.HermitCrab.FireMortar.mortarDamageCoefficient = 2.5f;
            EntityStates.HermitCrab.FireMortar.minimumDistance = 0f;
            EntityStates.HermitCrab.FireMortar.timeToTarget = 1.8f;
            EntityStates.HermitCrab.FireMortar.baseDuration = 0.7f;
            orig(self);
        }

        public static void GroundSwipe(On.EntityStates.GrandParentBoss.GroundSwipe.orig_OnEnter orig, EntityStates.GrandParentBoss.GroundSwipe self)
        {
            self.baseFireProjectileDelay = 1.5f;
            self.projectileHorizontalSpeed = 70f;
            orig(self);
        }

        public static void ChargeBombardment(On.EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.orig_OnEnter orig, EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment self)
        {
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.baseMaxChargeTime = 2.5f;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.maxCharges = 12;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.minGrenadeCount = 6;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.maxGrenadeCount = 30;
            EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.baseTotalDuration = 3f;
            orig(self);
        }

        public static void FireBombardment(On.EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.orig_OnEnter orig, EntityStates.ClayBoss.ClayBossWeapon.FireBombardment self)
        {
            self.grenadeCountMax = 30;
            EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.damageCoefficient = 0.8f;
            orig(self);
        }

        public static void FireVoidspikes(On.EntityStates.ImpBossMonster.FireVoidspikes.orig_OnEnter orig, EntityStates.ImpBossMonster.FireVoidspikes self)
        {
            EntityStates.ImpBossMonster.FireVoidspikes.walkSpeedPenaltyCoefficient = 1.2f;
            EntityStates.ImpBossMonster.FireVoidspikes.projectileCount = 10;
            EntityStates.ImpBossMonster.FireVoidspikes.damageCoefficient = 0.2f;
            orig(self);
        }

        public static void GroundPound(On.EntityStates.ImpBossMonster.GroundPound.orig_OnEnter orig, EntityStates.ImpBossMonster.GroundPound self)
        {
            EntityStates.ImpBossMonster.GroundPound.blastAttackRadius = 13f;
            orig(self);
        }

        public static void ChargeTrackingBomb(On.EntityStates.VagrantMonster.ChargeTrackingBomb.orig_OnEnter orig, EntityStates.VagrantMonster.ChargeTrackingBomb self)
        {
            EntityStates.VagrantMonster.ChargeTrackingBomb.baseDuration = 1.6f;
            orig(self);
        }

        public static void SpeedUpProjectiles(On.RoR2.Projectile.ProjectileSimple.orig_Start orig, ProjectileSimple self)
        {
            if (self.rigidbody && !self.rigidbody.useGravity && self.gameObject.GetComponent<TeamFilter>().teamIndex != TeamIndex.Player)
            {
                self.desiredForwardSpeed *= Main.ProjectileSpeed.Value;
                if (self.lifetime < 4f)
                {
                    self.lifetime = 4f;
                }
            }
            orig(self);
        }

        public static void FireFist2(On.EntityStates.TitanMonster.FireFist.orig_OnEnter orig, EntityStates.TitanMonster.FireFist self)
        {
            if (self is EntityStates.TitanMonster.FireGoldFist)
            {
                EntityStates.TitanMonster.FireFist.entryDuration = 1.6f;
                EntityStates.TitanMonster.FireFist.fireDuration = 1.1f;
                EntityStates.TitanMonster.FireFist.exitDuration = 1.6f;
            }
            orig(self);
        }

        public static void FireGoldFist(On.EntityStates.TitanMonster.FireGoldFist.orig_PlacePredictedAttack orig, EntityStates.TitanMonster.FireGoldFist self)
        {
            EntityStates.TitanMonster.FireGoldFist.fistCount = 10;
            EntityStates.TitanMonster.FireGoldFist.delayBetweenFists = 0.05f;
            orig(self);
        }

        public static void BlinkState2(On.EntityStates.ImpBossMonster.BlinkState.orig_OnEnter orig, EntityStates.ImpBossMonster.BlinkState self)
        {
            self.blastAttackDamageCoefficient = 7f;
            self.duration = 2f;
            self.destinationAlertDuration = 1.7f;
            self.exitDuration = 1.3f;
            self.blinkDistance = 9999f;
            orig(self);
        }

        public static void Flamebreath(On.EntityStates.LemurianBruiserMonster.Flamebreath.orig_OnEnter orig, EntityStates.LemurianBruiserMonster.Flamebreath self)
        {
            EntityStates.LemurianBruiserMonster.Flamebreath.baseEntryDuration = 0.5f;
            EntityStates.LemurianBruiserMonster.Flamebreath.baseExitDuration = 0.2f;
            EntityStates.LemurianBruiserMonster.Flamebreath.baseFlamethrowerDuration = 1.8f;
            EntityStates.LemurianBruiserMonster.Flamebreath.radius = 6f;
            EntityStates.LemurianBruiserMonster.Flamebreath.maxSpread = 6f;
            Debug.Log("hey handsome");
            orig(self);
        }

        public static void WeaponSlam2(On.EntityStates.BrotherMonster.WeaponSlam.orig_FixedUpdate orig, EntityStates.BrotherMonster.WeaponSlam self)
        {
            if (self.isAuthority)
            {
                if (self.hasDoneBlastAttack)
                {
                    if (self.modelTransform)
                    {
                        if (!Main.hasFired)
                        {
                            Ray ray = self.GetAimRay();
                            Transform transform = self.FindModelChild(EntityStates.BrotherMonster.Weapon.FireLunarShards.muzzleString);
                            if (transform)
                            {
                                ray.origin = transform.position;
                            }
                            var orbs = 7f;
                            Main.hasFired = true;
                            float orbCount = 360f / orbs;
                            Vector3 plane = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                            Transform transgender = self.FindModelChild(EntityStates.BrotherMonster.WeaponSlam.muzzleString);
                            Vector3 pos = transgender.position;
                            int j = 0;
                            while (j < orbs)
                            {
                                Vector3 zase = Quaternion.AngleAxis(orbCount * j, Vector3.up) * plane;
                                ProjectileManager.instance.FireProjectile(EntityStates.BrotherMonster.FistSlam.waveProjectilePrefab, pos, Util.QuaternionSafeLookRotation(zase), self.gameObject, self.characterBody.damage * EntityStates.BrotherMonster.FistSlam.waveProjectileDamageCoefficient * 0.6f, EntityStates.BrotherMonster.FistSlam.waveProjectileForce, Util.CheckRoll(self.characterBody.crit, self.characterBody.master), 0, null, -1f);
                                j++;
                            }
                            // i hate this code
                        }
                    }
                }
            }
            orig(self);
        }

        public static void UltChannelState(On.EntityStates.BrotherMonster.UltChannelState.orig_OnEnter orig, EntityStates.BrotherMonster.UltChannelState self)
        {
            EntityStates.BrotherMonster.UltChannelState.totalWaves = 8;
            if (Main.MithrixAS.Value)
            {
                EntityStates.BrotherMonster.UltChannelState.maxDuration = 8f / self.attackSpeedStat;
            }
            else
            {
                EntityStates.BrotherMonster.UltChannelState.maxDuration = 8f;
            }
            orig(self);
        }

        public static void Phase3(On.EntityStates.Missions.BrotherEncounter.Phase3.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase3 self)
        {
            Main.Ramp1.SetActive(false);
            Main.Ramp2.SetActive(false);
            Main.Ramp3.SetActive(false);
            Main.Rocks.SetActive(false);
            orig(self);
            if (NetworkServer.active)
            {
                foreach (TeamComponent teamComponent in new List<TeamComponent>(TeamComponent.GetTeamMembers(TeamIndex.Monster)))
                {
                    if (teamComponent)
                    {
                        HealthComponent component = teamComponent.GetComponent<HealthComponent>();
                        if (component)
                        {
                            switch (teamComponent.body.name)
                            {
                                case "LunarExploderBody(Clone)":
                                    component.Suicide(null, null, DamageType.Generic);
                                    break;

                                case "LunarGolemBody(Clone)":
                                    component.Suicide(null, null, DamageType.Generic);
                                    break;

                                case "LunarWispBody(Clone)":
                                    component.Suicide(null, null, DamageType.Generic);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void CleanupPillar(On.EntityStates.BrotherMonster.WeaponSlam.orig_OnEnter orig, EntityStates.BrotherMonster.WeaponSlam self)
        {
            var pillarprefab = EntityStates.BrotherMonster.WeaponSlam.pillarProjectilePrefab;
            pillarprefab.transform.localScale = new Vector3(1f, 1f, 1f);
            var ghost = pillarprefab.GetComponent<ProjectileController>().ghostPrefab;
            ghost.transform.localScale = new Vector3(1f, 1f, 1f);
            orig(self);
        }

        public static void AmbientLevelBoost(On.RoR2.Run.orig_RecalculateDifficultyCoefficentInternal orig, Run self)
        {
            orig(self);
            try
            {
                self.ambientLevel = (Main.LevelDiffBoostScaleWithPlayers.Value ? Mathf.Min(self.ambientLevel + Main.LevelDiffBoost.Value, int.MaxValue) : Mathf.Min(self.ambientLevel + (Main.LevelDiffBoost.Value / self.participatingPlayerCount), int.MaxValue));
            }
            catch
            {
                Main.InfernoLogger.LogError("Failed to fully apply Level and Difficulty boost");
            }
        }

        public static void Ambient(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld<Run>("ambientLevelCap")
            ))
            {
                c.EmitDelegate<Func<float, float>>((levelIn) =>
                {
                    float difficultyBoost = Main.LevelDiffBoost.Value;
                    Run.instance.compensatedDifficultyCoefficient += difficultyBoost * 0.05f; //stage 3 spawnrates at stage 0 monsoon, stage 2 spawnrates at stage 0 rainstorm
                    float levelOut = levelIn + difficultyBoost;
                    return levelOut;
                });
            }
            else
            {
                Main.InfernoLogger.LogError("Failed to fully apply Level and Difficulty boost");
            }
        }

        public static void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self)
        {
            orig(self);
            self.calcRadius += Self_calcRadius;
            self.calcChargeRate += Self_calcChargeRate;
            if (self.name.Contains("Battery"))
            {
                self.calcChargeRate += Self_calcChargeRate1;
            }
            self.calcColor += Self_calcColor;
        }

        private static void Self_calcChargeRate1(ref float rate)
        {
            rate *= 1f + Main.PillarSpeed.Value / 100f;
        }

        public static void Self_calcColor(ref Color color)
        {
            color = new Color(0.4117647059f, 0.1176470588f, 0.1450980392f, 1f) * 1.5f;
        }

        public static void Self_calcChargeRate(ref float rate)
        {
            rate *= 1f + Main.TeleporterSpeed.Value / 100f;
        }

        public static void Self_calcRadius(ref float radius)
        {
            radius *= Mathf.Max(1f + Main.TeleporterSize.Value / 100f, 0f);
        }

        public static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender && sender.teamComponent && sender.teamComponent.teamIndex == TeamIndex.Monster)
            {
                args.armorAdd += Main.LoopArmor.Value * Run.instance.loopClearCount;
                args.cooldownMultAdd += Main.StageCooldownReduction.Value * Run.instance.stageClearCount;

                if (sender.isBoss || sender.isChampion)
                {
                    args.healthMultAdd += Main.BossHp.Value * Run.instance.stageClearCount;
                }

                if (!sender.isBoss && !sender.isChampion)
                {
                    args.healthMultAdd += Main.MonsterHp.Value * Run.instance.stageClearCount;
                }
            }
        }

        public static void Wander_PickNewTargetLookPosition(ILContext il)
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
                Main.InfernoLogger.LogError("Failed to change AI Wander_PickNewTargetLookPosition");
            }
        }

        public static void LookBusy_PickNewTargetLookDirection(ILContext il)
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
                Main.InfernoLogger.LogError("Failed to change AI LookBusy_PickNewTargetLookPosition");
            }
        }

        public static void LookBusy_OnEnter(ILContext il)
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
                Main.InfernoLogger.LogError("Failed to change AI LookBusy_OnEnter");
            }
        }
    }
}