namespace Inferno.Skill_Misc
{
    public class HookBehavior
    {
        public static void ApplyHooks()
        {
            On.EntityStates.BasicMeleeAttack.OnEnter += Hooks.GupSpikesState;
            On.EntityStates.BeetleGuardMonster.FireSunder.OnEnter += Hooks.FireSunder;
            On.EntityStates.BeetleGuardMonster.GroundSlam.OnEnter += Hooks.GroundSlam;
            On.EntityStates.BeetleMonster.HeadbuttState.FixedUpdate += Hooks.HeadbuttState;
            On.EntityStates.BeetleQueenMonster.FireSpit.OnEnter += Hooks.FireSpit;
            On.EntityStates.BeetleQueenMonster.SpawnWards.OnEnter += Hooks.SpawnWards;
            On.EntityStates.Bison.Charge.OnEnter += Hooks.Charge;
            On.EntityStates.Bison.Headbutt.OnEnter += Hooks.Headbutt;
            On.EntityStates.BrotherHaunt.FireRandomProjectiles.OnEnter += Hooks.FireRandomProjectiles;
            if (Main.EnableMithrixChanges.Value)
            {
                On.EntityStates.BrotherMonster.BaseSlideState.OnEnter += Hooks.BaseSlideState;
                On.EntityStates.BrotherMonster.ExitSkyLeap.OnEnter += Hooks.ExitSkyLeap;
                On.EntityStates.BrotherMonster.FistSlam.OnEnter += Hooks.FistSlam;
                On.EntityStates.BrotherMonster.HoldSkyLeap.OnEnter += Hooks.HoldSkyLeap;
                On.EntityStates.BrotherMonster.SlideIntroState.OnEnter += Hooks.SlideIntroState;
                On.EntityStates.BrotherMonster.SpellChannelEnterState.OnEnter += Hooks.SpellChannelEnterState;
                On.EntityStates.BrotherMonster.SpellChannelExitState.OnEnter += Hooks.SpellChannelExitState;
                On.EntityStates.BrotherMonster.SpellChannelState.OnEnter += Hooks.SpellChannelState;
                On.EntityStates.BrotherMonster.SprintBash.OnEnter += Hooks.SprintBash;
                On.EntityStates.BrotherMonster.StaggerEnter.OnEnter += Hooks.StaggerEnter;
                On.EntityStates.BrotherMonster.StaggerExit.OnEnter += Hooks.StaggerExit;
                On.EntityStates.BrotherMonster.StaggerLoop.OnEnter += Hooks.StaggerLoop;
                On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += Hooks.TrueDeathState;
                On.EntityStates.BrotherMonster.UltChannelState.OnEnter += Hooks.UltChannelState;
                On.EntityStates.BrotherMonster.UltChannelState.FireWave += Hooks.FireWave;
                On.EntityStates.BrotherMonster.Weapon.FireLunarShards.OnEnter += Hooks.FireLunarShards;
                On.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate += Hooks.WeaponSlam2;
                On.EntityStates.BrotherMonster.WeaponSlam.OnEnter += Hooks.WeaponSlam;
            }

            On.EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.OnEnter += Hooks.ChargeBombardment;
            On.EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.OnEnter += Hooks.FireBombardment;
            On.EntityStates.ClayBoss.FireTarball.OnEnter += Hooks.FireTarBall;
            On.EntityStates.ClayBoss.PrepTarBall.OnEnter += Hooks.PrepTarBall;
            On.EntityStates.ClayBruiser.Weapon.FireSonicBoom.OnEnter += Hooks.FireSonicBoom;
            On.EntityStates.GenericCharacterSpawnState.OnEnter += Hooks.GenericCharacterSpawnState;
            On.EntityStates.GolemMonster.ChargeLaser.OnEnter += Hooks.ChargeLaser;
            On.EntityStates.GolemMonster.ClapState.OnEnter += Hooks.ClapState;
            On.EntityStates.GolemMonster.FireLaser.OnEnter += Hooks.FireLaser;
            On.EntityStates.GrandParentBoss.GroundSwipe.OnEnter += Hooks.GroundSwipe;
            On.EntityStates.GravekeeperBoss.FireHook.OnEnter += Hooks.FireHook;
            On.EntityStates.GravekeeperBoss.PrepHook.OnEnter += Hooks.PrepHook;
            On.EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.OnEnter += Hooks.GravekeeperBarrage;
            On.EntityStates.GreaterWispMonster.ChargeCannons.OnEnter += Hooks.ChargeCannons;
            On.EntityStates.HermitCrab.FireMortar.OnEnter += Hooks.FireMortar;
            On.EntityStates.ImpBossMonster.BlinkState.OnEnter += Hooks.BlinkState2;
            On.EntityStates.ImpBossMonster.FireVoidspikes.OnEnter += Hooks.FireVoidspikes;
            On.EntityStates.ImpBossMonster.GroundPound.OnEnter += Hooks.GroundPound;
            On.EntityStates.ImpMonster.BlinkState.OnEnter += Hooks.BlinkState;
            On.EntityStates.ImpMonster.DoubleSlash.OnEnter += Hooks.DoubleSlash;
            On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter += Hooks.FireMegaFireball;
            On.EntityStates.LemurianBruiserMonster.Flamebreath.OnEnter += Hooks.Flamebreath;
            On.EntityStates.LemurianMonster.Bite.FixedUpdate += Hooks.Bite2;
            On.EntityStates.LemurianMonster.Bite.OnEnter += Hooks.Bite1;
            On.EntityStates.LemurianMonster.ChargeFireball.OnEnter += Hooks.ChargeFireball;
            On.EntityStates.LemurianMonster.FireFireball.OnEnter += Hooks.FireFireball;
            On.EntityStates.LunarWisp.FireLunarGuns.OnEnter += Hooks.FireLunarGuns;
            On.EntityStates.MajorConstruct.Weapon.FireLaser.OnEnter += Hooks.FireLaser;
            On.EntityStates.MiniMushroom.SporeGrenade.OnEnter += Hooks.SporeGrenade;
            On.EntityStates.MinorConstruct.Weapon.ChargeConstructBeam.OnEnter += Hooks.ChargeConstructBeam;
            On.EntityStates.MinorConstruct.Weapon.FireConstructBeam.OnEnter += Hooks.FireConstructBeam;
            On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter += Hooks.Phase1;
            On.EntityStates.Missions.BrotherEncounter.Phase2.OnEnter += Hooks.Phase2;
            On.EntityStates.Missions.BrotherEncounter.Phase3.OnEnter += Hooks.Phase3;
            On.EntityStates.Missions.BrotherEncounter.Phase4.OnEnter += Hooks.Phase4;
            On.EntityStates.NullifierMonster.FirePortalBomb.OnEnter += Hooks.FirePortalBomb;
            On.EntityStates.TitanMonster.FireFist.OnEnter += Hooks.FireFist2;
            On.EntityStates.TitanMonster.FireFist.OnEnter += Hooks.FireFist;
            On.EntityStates.TitanMonster.FireGoldFist.PlacePredictedAttack += Hooks.FireGoldFist;
            On.EntityStates.VagrantMonster.ChargeTrackingBomb.OnEnter += Hooks.ChargeTrackingBomb;
            On.EntityStates.VoidBarnacle.Weapon.ChargeFire.OnEnter += Hooks.ChargeFire;
            On.EntityStates.VoidBarnacle.Weapon.Fire.OnEnter += Hooks.Fire;
            On.EntityStates.NullifierMonster.DeathState.OnEnter += Hooks.DeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter += Hooks.DeathState2;
            On.EntityStates.Wisp1Monster.FireEmbers.OnEnter += Hooks.FireEmbers;
            On.EntityStates.ClayGrenadier.FaceSlam.OnEnter += Hooks.FaceSlam;
            On.EntityStates.VagrantMonster.SpawnState.OnEnter += Hooks.SpawnState;

            On.RoR2.Projectile.ProjectileSimple.Start += Hooks.SpeedUpProjectiles;
            On.RoR2.Projectile.ProjectileController.Start += Hooks.SpeedUpProjectiles2;

            On.RoR2.Run.RecalculateDifficultyCoefficentInternal += Hooks.AmbientLevelBoost;
            On.RoR2.MasterSummon.Perform += Hooks.IncreaseMonsterCap;
            On.RoR2.CombatDirector.Awake += Hooks.CombatDirector_Awake;
            On.RoR2.HoldoutZoneController.Awake += Hooks.HoldoutZoneController_Awake;

            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter -= Hooks.CleanupPillar;
            On.EntityStates.NullifierMonster.DeathState.OnEnter -= Hooks.CleanupDeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter -= Hooks.CleanupDeathState2;

            On.RoR2.SceneDirector.Start += Hooks.CacheObjects;

            On.RoR2.HealthComponent.Awake += Hooks.HealthComponent_Awake;
        }

        public static void UndoHooks()
        {
            On.EntityStates.BasicMeleeAttack.OnEnter -= Hooks.GupSpikesState;
            On.EntityStates.BeetleGuardMonster.FireSunder.OnEnter -= Hooks.FireSunder;
            On.EntityStates.BeetleGuardMonster.GroundSlam.OnEnter -= Hooks.GroundSlam;
            On.EntityStates.BeetleMonster.HeadbuttState.FixedUpdate -= Hooks.HeadbuttState;
            On.EntityStates.BeetleQueenMonster.FireSpit.OnEnter -= Hooks.FireSpit;
            On.EntityStates.BeetleQueenMonster.SpawnWards.OnEnter -= Hooks.SpawnWards;
            On.EntityStates.Bison.Charge.OnEnter -= Hooks.Charge;
            On.EntityStates.Bison.Headbutt.OnEnter -= Hooks.Headbutt;
            On.EntityStates.BrotherHaunt.FireRandomProjectiles.OnEnter -= Hooks.FireRandomProjectiles;
            On.EntityStates.BrotherMonster.BaseSlideState.OnEnter -= Hooks.BaseSlideState;
            On.EntityStates.BrotherMonster.ExitSkyLeap.OnEnter -= Hooks.ExitSkyLeap;
            On.EntityStates.BrotherMonster.FistSlam.OnEnter -= Hooks.FistSlam;
            On.EntityStates.BrotherMonster.HoldSkyLeap.OnEnter -= Hooks.HoldSkyLeap;
            On.EntityStates.BrotherMonster.SlideIntroState.OnEnter += Hooks.SlideIntroState;
            On.EntityStates.BrotherMonster.SpellChannelEnterState.OnEnter -= Hooks.SpellChannelEnterState;
            On.EntityStates.BrotherMonster.SpellChannelExitState.OnEnter -= Hooks.SpellChannelExitState;
            On.EntityStates.BrotherMonster.SpellChannelState.OnEnter -= Hooks.SpellChannelState;
            On.EntityStates.BrotherMonster.SprintBash.OnEnter -= Hooks.SprintBash;
            On.EntityStates.BrotherMonster.StaggerEnter.OnEnter -= Hooks.StaggerEnter;
            On.EntityStates.BrotherMonster.StaggerExit.OnEnter -= Hooks.StaggerExit;
            On.EntityStates.BrotherMonster.StaggerLoop.OnEnter -= Hooks.StaggerLoop;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= Hooks.TrueDeathState;
            On.EntityStates.BrotherMonster.UltChannelState.OnEnter -= Hooks.UltChannelState;
            On.EntityStates.BrotherMonster.UltChannelState.FireWave -= Hooks.FireWave;
            On.EntityStates.BrotherMonster.Weapon.FireLunarShards.OnEnter -= Hooks.FireLunarShards;
            On.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate -= Hooks.WeaponSlam2;
            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter -= Hooks.WeaponSlam;
            On.EntityStates.ClayBoss.ClayBossWeapon.ChargeBombardment.OnEnter -= Hooks.ChargeBombardment;
            On.EntityStates.ClayBoss.ClayBossWeapon.FireBombardment.OnEnter -= Hooks.FireBombardment;
            On.EntityStates.ClayBoss.FireTarball.OnEnter -= Hooks.FireTarBall;
            On.EntityStates.ClayBoss.PrepTarBall.OnEnter -= Hooks.PrepTarBall;
            On.EntityStates.ClayBruiser.Weapon.FireSonicBoom.OnEnter -= Hooks.FireSonicBoom;
            On.EntityStates.GenericCharacterSpawnState.OnEnter -= Hooks.GenericCharacterSpawnState;
            On.EntityStates.GolemMonster.ChargeLaser.OnEnter -= Hooks.ChargeLaser;
            On.EntityStates.GolemMonster.ClapState.OnEnter -= Hooks.ClapState;
            On.EntityStates.GolemMonster.FireLaser.OnEnter -= Hooks.FireLaser;
            On.EntityStates.GrandParentBoss.GroundSwipe.OnEnter -= Hooks.GroundSwipe;
            On.EntityStates.GravekeeperBoss.FireHook.OnEnter -= Hooks.FireHook;
            On.EntityStates.GravekeeperBoss.PrepHook.OnEnter -= Hooks.PrepHook;
            On.EntityStates.GravekeeperMonster.Weapon.GravekeeperBarrage.OnEnter -= Hooks.GravekeeperBarrage;
            On.EntityStates.GreaterWispMonster.ChargeCannons.OnEnter -= Hooks.ChargeCannons;
            On.EntityStates.HermitCrab.FireMortar.OnEnter -= Hooks.FireMortar;
            On.EntityStates.ImpBossMonster.BlinkState.OnEnter -= Hooks.BlinkState2;
            On.EntityStates.ImpBossMonster.FireVoidspikes.OnEnter -= Hooks.FireVoidspikes;
            On.EntityStates.ImpBossMonster.GroundPound.OnEnter -= Hooks.GroundPound;
            On.EntityStates.ImpMonster.BlinkState.OnEnter -= Hooks.BlinkState;
            On.EntityStates.ImpMonster.DoubleSlash.OnEnter -= Hooks.DoubleSlash;
            On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter -= Hooks.FireMegaFireball;
            On.EntityStates.LemurianBruiserMonster.Flamebreath.OnEnter -= Hooks.Flamebreath;
            On.EntityStates.LemurianMonster.Bite.FixedUpdate -= Hooks.Bite2;
            On.EntityStates.LemurianMonster.Bite.OnEnter -= Hooks.Bite1;
            On.EntityStates.LemurianMonster.ChargeFireball.OnEnter -= Hooks.ChargeFireball;
            On.EntityStates.LemurianMonster.FireFireball.OnEnter -= Hooks.FireFireball;
            On.EntityStates.LunarWisp.FireLunarGuns.OnEnter -= Hooks.FireLunarGuns;
            On.EntityStates.MajorConstruct.Weapon.FireLaser.OnEnter -= Hooks.FireLaser;
            On.EntityStates.MiniMushroom.SporeGrenade.OnEnter -= Hooks.SporeGrenade;
            On.EntityStates.MinorConstruct.Weapon.ChargeConstructBeam.OnEnter -= Hooks.ChargeConstructBeam;
            On.EntityStates.MinorConstruct.Weapon.FireConstructBeam.OnEnter -= Hooks.FireConstructBeam;
            On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter -= Hooks.Phase1;
            On.EntityStates.Missions.BrotherEncounter.Phase2.OnEnter -= Hooks.Phase2;
            On.EntityStates.Missions.BrotherEncounter.Phase3.OnEnter -= Hooks.Phase3;
            On.EntityStates.Missions.BrotherEncounter.Phase4.OnEnter -= Hooks.Phase4;
            On.EntityStates.NullifierMonster.FirePortalBomb.OnEnter -= Hooks.FirePortalBomb;
            On.EntityStates.TitanMonster.FireFist.OnEnter -= Hooks.FireFist2;
            On.EntityStates.TitanMonster.FireFist.OnEnter -= Hooks.FireFist;
            On.EntityStates.TitanMonster.FireGoldFist.PlacePredictedAttack -= Hooks.FireGoldFist;
            On.EntityStates.VagrantMonster.ChargeTrackingBomb.OnEnter -= Hooks.ChargeTrackingBomb;
            On.EntityStates.VoidBarnacle.Weapon.ChargeFire.OnEnter -= Hooks.ChargeFire;
            On.EntityStates.VoidBarnacle.Weapon.Fire.OnEnter -= Hooks.Fire;
            On.EntityStates.NullifierMonster.DeathState.OnEnter -= Hooks.DeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter -= Hooks.DeathState2;
            On.EntityStates.Wisp1Monster.FireEmbers.OnEnter -= Hooks.FireEmbers;
            On.EntityStates.ClayGrenadier.FaceSlam.OnEnter -= Hooks.FaceSlam;
            On.EntityStates.VagrantMonster.SpawnState.OnEnter -= Hooks.SpawnState;

            On.RoR2.Projectile.ProjectileSimple.Start -= Hooks.SpeedUpProjectiles;
            On.RoR2.Projectile.ProjectileController.Start -= Hooks.SpeedUpProjectiles2;

            On.RoR2.Run.RecalculateDifficultyCoefficentInternal -= Hooks.AmbientLevelBoost;
            On.RoR2.MasterSummon.Perform -= Hooks.IncreaseMonsterCap;
            On.RoR2.CombatDirector.Awake -= Hooks.CombatDirector_Awake;
            On.RoR2.HoldoutZoneController.Awake -= Hooks.HoldoutZoneController_Awake;

            On.EntityStates.BrotherMonster.WeaponSlam.OnEnter += Hooks.CleanupPillar;
            On.EntityStates.NullifierMonster.DeathState.OnEnter += Hooks.CleanupDeathState;
            On.EntityStates.VoidMegaCrab.DeathState.OnEnter += Hooks.CleanupDeathState2;

            On.RoR2.SceneDirector.Start -= Hooks.CacheObjects;

            On.RoR2.HealthComponent.Awake -= Hooks.HealthComponent_Awake;
        }
    }
}