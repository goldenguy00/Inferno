using RoR2;
using RoR2.CharacterAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Inferno.Item;

namespace Inferno.Stat_AI
{
    public static class Master
    {
        public static void MasterChanges(CharacterMaster master)
        {
            switch (master.teamIndex)
            {
                default:
                    break;

                case TeamIndex.Monster:

                    //switch (master.bodyInstanceObject.GetComponent<CharacterBody>() == null)
                    switch (master.GetBody() == null)
                    {
                        case false:
                            var cb = master.bodyInstanceObject.GetComponent<CharacterBody>();
                            if (cb.isChampion)
                            {
                                master.inventory.GiveItem(RoR2Content.Items.TeleportWhenOob);
                            }
                            cb.levelMoveSpeed = Main.LevelMoveSpeed.Value;
                            cb.levelRegen = Main.LevelRegen.Value;
                            cb.levelAttackSpeed = Main.LevelAttackSpeed.Value;
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
                            ba.aimVectorDampTime = Mathf.Max(0.001f, 0.031f - (0.0001f * Main.AIScaling.Value * Run.instance.stageClearCount));
                            ba.aimVectorMaxSpeed = 250f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
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
                            ba.aimVectorDampTime = Mathf.Max(0.001f, 0.09f - (0.0003f * Main.AIScaling.Value * Run.instance.stageClearCount));
                            ba.aimVectorMaxSpeed = 250f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
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
                    BeetleQueenFireFuckwards.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    BeetleQueenFireFuckwards.maxUserHealthFraction = Mathf.Infinity;
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 25);
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
                    ImpOverlordGroundPound.maxDistance = 12f;

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
                    if (Main.EnableMithrixChanges.Value)
                    {
                        AISkillDriver MithrixFireShards = (from x in masterm.GetComponents<AISkillDriver>()
                                                           where x.customName == "Sprint and FireLunarShards"
                                                           select x).First();
                        MithrixFireShards.minDistance = 0f;
                        MithrixFireShards.maxUserHealthFraction = Mathf.Infinity;

                        AISkillDriver MithrixSprint = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.customName == "Sprint After Target"
                                                       select x).First();
                        MithrixSprint.minDistance = 40f - (Main.AIScaling.Value * Run.instance.stageClearCount);

                        /*
                        AISkillDriver DashForward = (from x in masterm.GetComponents<AISkillDriver>()
                                                     where x.customName == "DashForward"
                                                     select x).First();
                        DashForward.minDistance = 30f;
                        DashForward.maxDistance = 45f;
                        */

                        AISkillDriver DashStrafe = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "DashStrafe"
                                                    select x).First();
                        // DashStrafe.maxDistance = 30f;
                        DashStrafe.nextHighPriorityOverride = MithrixFireShards;

                        //master.inventory.GiveItem(Items.PrimaryStockItemDef, 1);
                        master.inventory.GiveItem(Items.UtilityStockItemDef, 1);
                        master.inventory.GiveItem(Items.SpecialStockItemDef, 1);
                        master.inventory.GiveItem(Items.AllCooldownItemDef, 10);
                    }

                    break;

                case "BrotherHurtMaster(Clone)":
                    if (Main.EnableMithrixChanges.Value)
                    {
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
                        master.inventory.GiveItem(Items.PrimaryStockItemDef, 6);
                        master.inventory.GiveItem(Items.AllCooldownItemDef, 5);
                    }
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
                    AlphaConstructFire.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    break;

                case "BeetleMaster(Clone)":
                    AISkillDriver BeetleHeadbutt = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "HeadbuttOffNodegraph"
                                                    select x).First();
                    BeetleHeadbutt.maxDistance = 25f + (1f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    BeetleHeadbutt.selectionRequiresOnGround = true;
                    BeetleHeadbutt.activationRequiresAimTargetLoS = true;
                    break;

                case "BeetleGuardMaster(Clone)":
                    AISkillDriver BeetleGuardFireSunder = (from x in masterm.GetComponents<AISkillDriver>()
                                                           where x.customName == "FireSunder"
                                                           select x).First();
                    BeetleGuardFireSunder.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    //master.inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine, 2);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 25);
                    master.inventory.GiveItem(Items.SecondaryStockItemDef, 2);
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
                    GreaterWispShoot.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
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
                    LemurianShoot.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);

                    AISkillDriver LemurianStrafe = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "StrafeIdley"
                                                    select x).First();

                    LemurianStrafe.minDistance = 10f;
                    LemurianStrafe.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
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
                    LunarExploderShoot.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    AISkillDriver LunarExploderSprintShoot = (from x in masterm.GetComponents<AISkillDriver>()
                                                              where x.customName == "SprintNodegraphAndShoot"
                                                              select x).First();
                    LunarExploderSprintShoot.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 5);
                    break;

                case "GolemMaster(Clone)":
                    AISkillDriver StoneGolemShootLaser = (from x in masterm.GetComponents<AISkillDriver>()
                                                          where x.skillSlot == SkillSlot.Secondary
                                                          select x).First();
                    StoneGolemShootLaser.selectionRequiresAimTarget = true;
                    StoneGolemShootLaser.activationRequiresAimTargetLoS = true;
                    StoneGolemShootLaser.activationRequiresAimConfirmation = true;
                    StoneGolemShootLaser.maxDistance = 100f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
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
                    XiConstructLazer.maxDistance = 60f;
                    XiConstructLazer.minDistance = 0f;
                    AISkillDriver XiConstructShield = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.skillSlot == SkillSlot.Utility
                                                       select x).First();
                    XiConstructShield.maxDistance = 60f;
                    XiConstructShield.minDistance = 0f;
                    AISkillDriver XiConstructSummon = (from x in masterm.GetComponents<AISkillDriver>()
                                                       where x.skillSlot == SkillSlot.Special
                                                       select x).First();
                    XiConstructSummon.maxDistance = 60f;
                    XiConstructSummon.minDistance = 0f;
                    AISkillDriver XiConstructStrafeStep = (from x in masterm.GetComponents<AISkillDriver>()
                                                           where x.customName == "StrafeStep"
                                                           select x).First();
                    XiConstructSummon.maxDistance = 60f;
                    XiConstructSummon.minDistance = 0f;
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
                    ClayApothecaryFaceslam.maxDistance = 35f + (7f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 33);
                    break;

                case "ParentMaster(Clone)":
                    AISkillDriver ParentTeleport = (from x in masterm.GetComponents<AISkillDriver>()
                                                    where x.customName == "Teleport"
                                                    select x).First();
                    ParentTeleport.maxUserHealthFraction = 1f;
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 33);
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
                    BisonCharge.maxDistance = 150f + (20f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 20);
                    break;

                case "VultureMaster(Clone)":
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 25);
                    break;

                case "MiniMushroomMaster(Clone)":
                    AISkillDriver SporeGrenade = (from x in masterm.GetComponents<AISkillDriver>()
                                                  where x.customName == "Spore Grenade"
                                                  select x).First();
                    SporeGrenade.maxDistance = 60f + (10f * Main.AIScaling.Value * Run.instance.stageClearCount);
                    AISkillDriver MushrumPath = (from x in masterm.GetComponents<AISkillDriver>()
                                                 where x.customName == "Path"
                                                 select x).First();
                    MushrumPath.shouldSprint = true;
                    AISkillDriver PathStrafe = (from x in masterm.GetComponents<AISkillDriver>()
                                                where x.customName == "PathStrafe"
                                                select x).First();
                    PathStrafe.shouldSprint = true;
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 25);
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
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 25);
                    break;

                case "LunarGolemMaster(Clone)":
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 12);
                    break;

                case "VoidMegaCrabMaster(Clone)":
                    //master.inventory.GiveItem(RoR2Content.Items.AlienHead, 1);
                    master.inventory.GiveItem(Items.AllCooldownItemDef, 33);
                    break;

                case "RoboBallBossMaster(Clone)":
                    AISkillDriver EnableEyebeam = (from x in masterm.GetComponents<AISkillDriver>()
                                                   where x.skillSlot == SkillSlot.Special
                                                   select x).First();
                    EnableEyebeam.maxUserHealthFraction = 0.5f;
                    master.inventory.GiveItem(Items.PrimaryStockItemDef, 1);
                    break;

                case "ScavMaster(Clone)":
                    AISkillDriver Sit = (from x in masterm.GetComponents<AISkillDriver>()
                                         where x.customName == "Sit"
                                         select x).First();
                    Sit.maxUserHealthFraction = 0.75f;
                    AISkillDriver FireCannon = (from x in masterm.GetComponents<AISkillDriver>()
                                                where x.customName == "FireCannon"
                                                select x).First();
                    FireCannon.maxDistance = 100f;
                    AISkillDriver ThrowSack = (from x in masterm.GetComponents<AISkillDriver>()
                                               where x.customName == "ThrowSack"
                                               select x).First();
                    ThrowSack.maxDistance = 100f;
                    AISkillDriver UseEquipmentAndFireCannon = (from x in masterm.GetComponents<AISkillDriver>()
                                                               where x.customName == "UseEquipmentAndFireCannon"
                                                               select x).First();
                    UseEquipmentAndFireCannon.maxDistance = 100f;
                    break;

                case "VoidBarnacleMaster(Clone)":
                    AISkillDriver Shooty = (from x in masterm.GetComponents<AISkillDriver>()
                                            where x.customName == "Shooty"
                                            select x).First();
                    Shooty.maxDistance = 100f;
                    break;

                case "SuperRoboBallBossMaster(Clone)":
                    AISkillDriver FireAndStop = (from x in masterm.GetComponents<AISkillDriver>()
                                                 where x.customName == "FireAndStop"
                                                 select x).First();
                    FireAndStop.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                    break;
            }
        }
    }
}