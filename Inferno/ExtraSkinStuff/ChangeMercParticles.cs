using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ism = InfernoSkinMod;

namespace Inferno.ExtraSkinStuff
{
    public static class ChangeMercParticles
    {
        /*
        public static GameObject exposeObject;
        public static Material vanillaMatMercExposed;
        public static Material vanillaMatMercExposedBackdrop;
        public static ParticleSystemRenderer editedMatMercExposed;
        public static ParticleSystemRenderer editedMatMercExposedBackdrop;

        public static Material InfernoMatMercExposed;

        public static Texture2D texRampHuntressSoft = null;
        public static Texture2D texRampHuntressSoftRed = null;

        public static Material matMercDelayedBillboard2Red = null;
        public static Material matMercFocusedAssaultIconRed = null;
        public static Material matMercExposedBackdropRed = null;

        public static Material matMercSwipe1Red = null;
        public static Material matMercSwipe2Red = null;
        public static Material matMercSwipe3Red = null;

        public static Material matMercIgnitionRed = null;

        public static Material matMercExposedSlashRed = null;
        public static Material matOmniHitspark3MercRed = null;
        public static Material matOmniRadialSlash1MercRed = null;
        public static Material matOmniHitspark4MercRed = null;

        public static Material matMercEnergized = LegacyResourcesAPI.Load<Material>("materials/matMercEnergized");
        public static Material matMercEnergizedRed = Object.Instantiate(matMercEnergized);

        //public static Material matMercEvisTargetRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget");
        //public static Material matMercEvisTarget = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget");
        public static Material matMercHologramRed = LegacyResourcesAPI.Load<Material>("materials/matMercHologram");

        //public static Material matMercHologram = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercHologram");

        //public static Material matHuntressFlashBrightRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright");
        public static Material matHuntressFlashBright = LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright");

        //public static Material matHuntressFlashExpandedRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded");
        public static Material matHuntressFlashExpanded = LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded");

        public static GameObject GlowFlowerForPillar = null;

        public static GameObject MercFocusedAssaultOrbEffectRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/MercFocusedAssaultOrbEffect"), "MercFocusedAssaultOrbEffectRed", false);

        public static GameObject OmniImpactVFXSlashMerc = null; //Primary, Secondary1, Secondary2

        //public static GameObject MercSwordSlash = null; //Primary
        public static GameObject MercSwordFinisherSlash = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwind = null;  //Secondary1
        //public static GameObject MercDashHitOverlay = null; //Utility1, Utility2

        public static GameObject MercSwordUppercutSlash = null;  //Secondary2

        public static GameObject OmniImpactVFXSlashMercRed = null; //Primary, Secondary1, Secondary2
        public static GameObject MercSwordSlashRed = null; //Primary
        public static GameObject MercSwordFinisherSlashRed = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwindRed = null;  //Secondary1
        public static GameObject MercDashHitOverlayRed = null; //Utility1, Utility2

        public static GameObject HuntressBlinkEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/HuntressBlinkEffect"); //Special1 (Enter)
        public static GameObject HuntressBlinkEffectRed = PrefabAPI.InstantiateClone(HuntressBlinkEffect, "HuntressBlinkEffectRed", false); //Special1 (Enter)

        public static GameObject HuntressFireArrowRain = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/HuntressFireArrowRain"); //Special1 (Attack)
        public static GameObject HuntressFireArrowRainRed = PrefabAPI.InstantiateClone(HuntressFireArrowRain, "HuntressFireArrowRainRed", false); //Special1 (Attack)

        public static GameObject OmniImpactVFXSlashMercEvis = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/omnieffect/OmniImpactVFXSlashMercEvis"); //Special1 (Attack)
        public static GameObject OmniImpactVFXSlashMercEvisRed = PrefabAPI.InstantiateClone(OmniImpactVFXSlashMercEvis, "OmniImpactVFXSlashMercEvisRed", false); //Special1 (Attack)

        public static Material matHuntressSwipeRed;
        public static Material matHuntressChargedRed;

        public static GameObject MercSwordUppercutSlashRed = null;  //Secondary2
        public static GameObject ImpactMercFocusedAssaultRed = null;  //Utility2
        public static GameObject ImpactMercAssaulterRed = null;  //Utility2
        public static GameObject MercAssaulterEffectRed = null;  //Utility2

        public static GameObject EvisProjectileRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/EvisProjectile"), "EvisProjectileRed", true);  //Special2
        public static GameObject EvisProjectileGhostRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisProjectileGhost"), "EvisProjectileGhostRed", false);
        public static GameObject EvisOverlapProjectileRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/EvisOverlapProjectile"), "EvisOverlapProjectileRed", true);
        public static GameObject EvisOverlapProjectileGhostRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisOverlapProjectileGhost"), "EvisOverlapProjectileGhostRed", false);

        public static GameObject ImpactMercEvisRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/ImpactMercEvis"), "ImpactMercEvisRed", false);

        public static Material matMercExposed;
        public static Material matMercExposedRed;
        public static Material matMercExposedBackdrop;

        public static GameObject MercExposeEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/temporaryvisualeffects/MercExposeEffect");
        public static GameObject MercExposeConsumeEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/MercExposeConsumeEffect");
        public static GameObject MercExposeEffectRed = PrefabAPI.InstantiateClone(MercExposeEffect, "MercExposeEffectRed", false);
        public static GameObject MercExposeConsumeEffectRed = PrefabAPI.InstantiateClone(MercExposeConsumeEffect, "MercExposeConsumeEffectRed", false);

        public static string s = "Assets/InfernoSkins";

        public static void Hooks()
        {
            MercSwordSlashRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion(), "MercSwordSlashRed", false);

            MercSwordFinisherSlash = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion();
            MercSwordFinisherSlashRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion(), "MercSwordFinisherSlashRed", false);

            MercSwordSlashWhirlwind = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion();
            MercSwordSlashWhirlwindRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion(), "MercSwordSlashWhirlwindRed", false);
            ContentAddition.AddEffect(MercSwordSlashWhirlwindRed);

            MercSwordUppercutSlash = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion();
            MercSwordUppercutSlashRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion(), "MercSwordUppercutSlashRed", false);
            ContentAddition.AddEffect(MercSwordUppercutSlashRed);

            OmniImpactVFXSlashMerc = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion();
            OmniImpactVFXSlashMercRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion(), "OmniImpactVFXSlashMercRed", false);
            ContentAddition.AddEffect(OmniImpactVFXSlashMercRed);

            //OmniImpactVFXSlashMerc = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion();
            MercAssaulterEffectRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion(), "MercAssaulterEffectRed", false);
            //R2API.ContentAddition.AddEffect(MercAssaulterEffectRed);

            MercDashHitOverlayRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercDashHitOverlay.prefab").WaitForCompletion(), "MercDashHitOverlayRed", false);
            //R2API.ContentAddition.AddEffect(MercDashHitOverlayRed);

            ImpactMercAssaulterRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercAssaulter.prefab").WaitForCompletion(), "ImpactMercAssaulterRed", false);
            ContentAddition.AddEffect(ImpactMercAssaulterRed);

            ImpactMercFocusedAssaultRed = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercFocusedAssault.prefab").WaitForCompletion(), "ImpactMercFocusedAssaultRed", false);
            ContentAddition.AddEffect(ImpactMercFocusedAssaultRed);

            exposeObject = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercExposeEffect.prefab").WaitForCompletion();
            editedMatMercExposed = exposeObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            editedMatMercExposedBackdrop = exposeObject.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>();

            vanillaMatMercExposed = Addressables.LoadAssetAsync<Material>("RoR2/Base/Merc/matMercExposed.mat").WaitForCompletion();
            vanillaMatMercExposedBackdrop = Addressables.LoadAssetAsync<Material>("RoR2/Base/Merc/matMercExposedBackdrop.mat").WaitForCompletion();

            InfernoMatMercExposed = Main.inferno.LoadAsset<Material>("matMercExposed");

            // On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;

            Texture2D texRampFallbootsRed = Main.inferno.LoadAsset<Sprite>(s + "texRampFallboots.png").texture;

            Texture2D texRampMercDustRed = Main.inferno.LoadAsset<Sprite>(s + "texRampMercDust.png").texture;

            ParticleSystemRenderer MercSwordSlashRedRenderer0 = MercSwordSlashRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercSwipe1Red = Object.Instantiate(MercSwordSlashRedRenderer0.material);
            matMercSwipe1Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercSwipe1Red.SetColor("_TintColor", new Color32(0, 59, 255, 255)); // 0, 80, 255, 255
            MercSwordSlashRedRenderer0.material = matMercSwipe1Red;

            //Child 0 has weird TracerBright
            ParticleSystem MercSwordFinisherSlashRedParticle0 = MercSwordFinisherSlashRed.transform.GetChild(0).GetComponent<ParticleSystem>();
            MercSwordFinisherSlashRedParticle0.startColor = new Color32(5, 130, 255, 255); // 0, 149, 255, 255
            ParticleSystemRenderer MercSwordFinisherSlashRedRenderer1 = MercSwordFinisherSlashRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercSwipe2Red = Object.Instantiate(MercSwordFinisherSlashRedRenderer1.material);
            matMercSwipe2Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercSwipe2Red.SetColor("_TintColor", new Color32(94, 155, 255, 255));  // 93, 168, 255, 255
            MercSwordFinisherSlashRedRenderer1.material = matMercSwipe2Red;

            ParticleSystemRenderer MercSwordSlashWhirlwindRedRenderer0 = MercSwordSlashWhirlwindRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            MercSwordSlashWhirlwindRedRenderer0.material = matMercSwipe1Red;

            ParticleSystemRenderer MercSwordUppercutSlashRedRenderer0 = MercSwordUppercutSlashRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer MercSwordUppercutSlashRedRenderer1 = MercSwordUppercutSlashRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            MercSwordUppercutSlashRedRenderer0.material = matMercSwipe2Red;
            MercSwordUppercutSlashRedRenderer1.material = matMercSwipe1Red;

            OmniEffect OmniImpactVFXSlashMercRedOmniEffect = OmniImpactVFXSlashMercRed.GetComponent<OmniEffect>();

            //Material matOmniHitspark4Merc = Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial);
            //OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc;

            Material matOmniRadialSlash1Merc = Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc;

            Material matOmniHitspark3Merc = Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniHitspark3Merc.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc;

            Material matOmniHitspark2MercRed = Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial);
            matOmniHitspark2MercRed.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2MercRed;

            ParticleSystem OmniImpactVFXSlashMercRedParticle1 = OmniImpactVFXSlashMercRed.transform.GetChild(1).GetComponent<ParticleSystem>(); //matOmniHitspark3 (Instance)
            OmniImpactVFXSlashMercRedParticle1.startColor = new Color32(0, 174, 183, 255); // 0, 185, 179, 255

            ParticleSystem OmniImpactVFXSlashMercRedParticle2 = OmniImpactVFXSlashMercRed.transform.GetChild(2).GetComponent<ParticleSystem>(); //matGenericFlash (Instance)
            OmniImpactVFXSlashMercRedParticle2.startColor = new Color32(0, 106, 237, 255); // 0, 126, 238, 255

            ParticleSystem OmniImpactVFXSlashMercRedParticle3 = OmniImpactVFXSlashMercRed.transform.GetChild(3).GetComponent<ParticleSystem>(); //matTracerBright (Instance)
            OmniImpactVFXSlashMercRedParticle3.startColor = new Color32(90, 107, 13, 255); // 0.3854 0.4245 0.0501 1 || 98, 108, 13, 255

            //Figure out if start color needs to actually be changed because they all use it
            ParticleSystemRenderer MercAssaulterEffectRedRenderer5 = MercAssaulterEffectRed.transform.GetChild(5).GetComponent<ParticleSystemRenderer>();
            matMercIgnitionRed = Object.Instantiate(MercAssaulterEffectRedRenderer5.material);
            matMercIgnitionRed.SetTexture("_RemapTex", texRampHuntressRed);
            matMercIgnitionRed.SetColor("_TintColor", new Color32(0, 18, 224, 255)); // 0, 35, 226, 255
            MercAssaulterEffectRedRenderer5.material = matMercIgnitionRed;

            MercAssaulterEffectRed.transform.GetChild(6).GetComponent<ParticleSystem>().startColor = new Color32(0, 91, 249, 255); //0, 112, 250, 255
            MercAssaulterEffectRed.transform.GetChild(8).GetComponent<Light>().color = new Color32(97, 112, 249, 255); // 96, 124, 250, 255
            MercAssaulterEffectRed.transform.GetChild(9).GetComponent<TrailRenderer>().material = matMercSwipe1Red;
            MercAssaulterEffectRed.transform.GetChild(10).GetChild(2).GetComponent<TrailRenderer>().material = matMercIgnitionRed;
            MercAssaulterEffectRed.transform.GetChild(10).GetChild(3).GetComponent<TrailRenderer>().material = matMercIgnitionRed;

            ////////////////////////

            ParticleSystem particleSystem = ImpactMercAssaulterRed.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.45f, 0.45f, 1f);//0.3538 0.6316 1 1
            particleSystem = ImpactMercAssaulterRed.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.575f, 0.575f, 1f);//0.467 0.7022 1 1

            ParticleSystemRenderer particleSystemRenderer = ImpactMercAssaulterRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercSwipe1Red;

            particleSystem = ImpactMercFocusedAssaultRed.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.45f, 0.45f, 0.2667f);//0.3538 0.6316 1 0.2667
            particleSystem = ImpactMercFocusedAssaultRed.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.934f, 0.26f, 0.26f, 1f);//0.0925 0.4637 0.934 1
            particleSystemRenderer = ImpactMercFocusedAssaultRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercSwipe3Red = Object.Instantiate(particleSystemRenderer.material);
            matMercSwipe3Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercSwipe3Red;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercDelayedBillboard2Red = Object.Instantiate(particleSystemRenderer.material);
            matMercDelayedBillboard2Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercDelayedBillboard2Red;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercFocusedAssaultIconRed = Object.Instantiate(particleSystemRenderer.material);
            matMercFocusedAssaultIconRed.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercFocusedAssaultIconRed;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercExposedBackdropRed = Object.Instantiate(particleSystemRenderer.material);
            matMercExposedBackdrop = Object.Instantiate(particleSystemRenderer.material);
            matMercExposedBackdropRed.SetColor("_TintColor", new Color(6, 0, 0, 0.5f));
            particleSystemRenderer.material = matMercExposedBackdropRed;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            matMercExposedSlashRed = Object.Instantiate(particleSystemRenderer.material);
            matMercExposedSlashRed.SetTexture("_RemapTex", texRampHuntressRed);
            matMercExposedSlashRed.SetColor("_TintColor", new Color(0.8868f, 0.06f, 0.06f, 1)); //r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedSlashRed;

            ContentAddition.AddEffect(MercFocusedAssaultOrbEffectRed);

            matMercEnergizedRed.SetTexture("_RemapTex", texRampHuntressSoftRed);
            matMercEnergizedRed.SetColor("_TintColor", new Color(1.8f, 0.35f, 0.35f, 1));

            //HuntressBlinkEffectRed
            particleSystem = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.6324f, 0.154f, 0.154f, 1f);//0.1534 0.1567 0.6324 1
            //particleSystem.colorOverLifetime.SetPropertyValue<bool>("enabled", false);
            var stupid = particleSystem.colorOverLifetime;
            stupid.enabled = false;
            //particleSystem.SetPropertyValue("colorOverLifetime", stupid);

            Light light = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(2).GetComponent<Light>();
            light.color = new Color(1f, 0.6f, 0.6f, 1); //0.2721 0.9699 1 1

            particleSystemRenderer = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressSwipeRed = Object.Instantiate(particleSystemRenderer.material);
            matHuntressSwipeRed.SetTexture("_RemapTex", texRampHuntressSoftRed);
            particleSystemRenderer.material = matHuntressSwipeRed;

            particleSystemRenderer = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(5).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipeRed;
            ContentAddition.AddEffect(HuntressBlinkEffectRed);
            //

            //HuntressFireArrowRainRed
            particleSystemRenderer = HuntressFireArrowRainRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipeRed;

            particleSystemRenderer = HuntressFireArrowRainRed.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipeRed;

            particleSystemRenderer = HuntressFireArrowRainRed.transform.GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressChargedRed = Object.Instantiate(particleSystemRenderer.material);
            matHuntressChargedRed.SetTexture("_RemapTex", texRampHuntressRed);
            particleSystemRenderer.material = matHuntressChargedRed;

            light = HuntressFireArrowRainRed.transform.GetChild(5).GetComponent<Light>();
            light.color = new Color(1f, 0.55f, 0.55f, 1f); //0.3456 0.7563 1 1
            ContentAddition.AddEffect(HuntressFireArrowRainRed);
            //

            //OmniImpactVFXSlashMercEvisRed
            OmniEffect omniEffect = OmniImpactVFXSlashMercEvisRed.GetComponent<OmniEffect>();

            omniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4MercRed;

            matOmniRadialSlash1MercRed = Object.Instantiate(omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1MercRed.SetTexture("_RemapTex", texRampMercDustRed);

            omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1MercRed;

            omniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3MercRed;
            omniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2MercRed;

            particleSystemRenderer = OmniImpactVFXSlashMercEvisRed.transform.GetChild(7).GetComponent<ParticleSystemRenderer>();
            matMercHologramRed = Object.Instantiate(particleSystemRenderer.material);
            matMercHologramRed.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercHologramRed.SetColor("_TintColor", new Color(1.825f, -0.25f, 0f, 1f));//0.2842 0.4328 1.826 1
            particleSystemRenderer.material = matMercHologramRed;

            ContentAddition.AddEffect(OmniImpactVFXSlashMercEvisRed);

            EvisProjectileRed.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = EvisProjectileGhostRed;
            EvisProjectileRed.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect = MercSwordFinisherSlashRed;
            EvisProjectileRed.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().childrenProjectilePrefab = EvisOverlapProjectileRed;
            ContentAddition.AddProjectile(EvisProjectileRed);

            EvisOverlapProjectileRed.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = EvisOverlapProjectileGhostRed;
            EvisOverlapProjectileRed.GetComponent<RoR2.Projectile.ProjectileOverlapAttack>().impactEffect = ImpactMercEvisRed;

            EvisProjectileGhostRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2Red;
            EvisProjectileGhostRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1Red;
            EvisProjectileGhostRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercIgnitionRed;
            EvisProjectileGhostRed.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f); //0 0.5827 1 1
            EvisProjectileGhostRed.transform.GetChild(5).GetComponent<Light>().color = new Color(1f, 0.3f, 0.3f, 1f); //0.1274 0.4704 1 1

            EvisOverlapProjectileGhostRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2Red;
            EvisOverlapProjectileGhostRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matOmniRadialSlash1MercRed;
            EvisOverlapProjectileGhostRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matOmniHitspark2MercRed;
            EvisOverlapProjectileGhostRed.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.51f, 0.51f, 1f);//0.3066 0.7276 1 1
            EvisOverlapProjectileGhostRed.transform.GetChild(4).GetComponent<Light>().color = new Color(1f, 0.3f, 0.3f, 1f);//0.1274 0.4704 1 1
            EvisOverlapProjectileGhostRed.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().material = matMercHologramRed;
            EvisOverlapProjectileGhostRed.transform.GetChild(6).GetComponent<ParticleSystemRenderer>().material = matMercHologramRed;

            ImpactMercEvisRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercHologramRed;
            ImpactMercEvisRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1Red;
            ImpactMercEvisRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1Red;
            ImpactMercEvisRed.transform.GetChild(3).GetComponent<Light>().color = new Color(1f, 0.425f, 0.425f, 1f);//0 0.8542 1 1

            ContentAddition.AddEffect(ImpactMercEvisRed);

            particleSystemRenderer = MercExposeEffectRed.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercExposed = Object.Instantiate(particleSystemRenderer.material);
            matMercExposedRed = Object.Instantiate(particleSystemRenderer.material);
            matMercExposedRed.SetTexture("_RemapTex", texRampHuntressRed);
            matMercExposedRed.SetColor("_TintColor", new Color(0.9f, 0.06f, 0.06f, 1f));//r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedRed;
            particleSystemRenderer = MercExposeEffectRed.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedBackdropRed;

            particleSystemRenderer = MercExposeConsumeEffectRed.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedRed;
            particleSystemRenderer = MercExposeConsumeEffectRed.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedSlashRed;

            SkinDef SkinDefMercOni = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Merc/skinMercAlt.asset").WaitForCompletion();

            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                orig(self, damageInfo);

                if (damageInfo.damageType == DamageType.ApplyMercExpose)
                {
                    uint skinindex = damageInfo.attacker.GetComponent<CharacterBody>().skinIndex;
                    if (skinindex != MercSkinExpose)
                    {
                        if (skinindex == 1)
                        {
                            MercExposeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposedRed;
                            MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.225f, 0.125f, 0.125f, 0.275f);//0.1335 0.1455 0.2264 0.3412
                            MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdropRed;

                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposedRed;
                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdropRed;
                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.35f, 0.2f, 0.2f, 0.175f);

                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposedRed;
                        }
                        else
                        {
                            MercExposeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed;
                            MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.1335f, 0.1455f, 0.2264f, 0.325f);
                            MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop;

                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed;
                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop;
                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.1076f, 0.2301f, 0.3868f, 0.25f);
                            MercExposeConsumeEffect.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposed;
                        }
                    }
                }
            };

            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex == 1)
                {
                    self.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                    self.swingEffectPrefab = MercSwordSlashRed;
                    if (self.isComboFinisher == true)
                    {
                        self.ignoreAttackSpeed = true;
                    }
                    EntityStates.Merc.Weapon.GroundLight2.comboFinisherSwingEffectPrefab = MercSwordFinisherSlashRed;
                }
                else
                {
                    EntityStates.Merc.Weapon.GroundLight2.comboFinisherSwingEffectPrefab = MercSwordFinisherSlash;
                }
                if (self.isComboFinisher == true)
                {
                    self.ignoreAttackSpeed = true;
                }
                orig(self);
            };

            On.EntityStates.Merc.WhirlwindBase.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex != MercSkinSecondary)
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1) //Replace this with a check for skinDef
                    {
                        EntityStates.Merc.WhirlwindBase.swingEffectPrefab = MercSwordSlashWhirlwindRed;
                        EntityStates.Merc.WhirlwindBase.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                    }
                    else
                    {
                        EntityStates.Merc.WhirlwindBase.swingEffectPrefab = MercSwordSlashWhirlwind;
                        EntityStates.Merc.WhirlwindBase.hitEffectPrefab = OmniImpactVFXSlashMerc;
                    }
                }
                orig(self);
            };

            On.EntityStates.Merc.Uppercut.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex != MercSkinSecondaryAlt)
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        EntityStates.Merc.Uppercut.swingEffectPrefab = MercSwordUppercutSlashRed;
                        EntityStates.Merc.Uppercut.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                    }
                    else
                    {
                        EntityStates.Merc.Uppercut.swingEffectPrefab = MercSwordUppercutSlash;
                        EntityStates.Merc.Uppercut.hitEffectPrefab = OmniImpactVFXSlashMerc;
                    }
                }
                orig(self);
            };

            On.EntityStates.Merc.Assaulter2.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex == 1)
                {
                    self.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                    self.swingEffectPrefab = MercAssaulterEffectRed;
                    //EntityStates.Merc.Assaulter2.selfOnHitOverlayEffectPrefab = MercDashHitOverlayRed;

                    matMercEnergized.SetTexture("_RemapTex", texRampHuntressSoftRed);
                    matMercEnergized.SetColor("_TintColor", new Color(1.8f, 0.35f, 0.35f, 1));
                    orig(self);
                    matMercEnergized.SetTexture("_RemapTex", texRampHuntressSoft);
                    matMercEnergized.SetColor("_TintColor", new Color(0.2842f, 0.4328f, 1.826f, 1));
                    return;
                }
                orig(self);
            };

            On.EntityStates.Merc.FocusedAssaultDash.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex == 1)
                {
                    self.delayedEffectPrefab = ImpactMercFocusedAssaultRed;
                    self.hitEffectPrefab = ImpactMercAssaulterRed;
                    //self.selfOnHitOverlayEffectPrefab = MercDashHitOverlayRed;
                    self.swingEffectPrefab = MercAssaulterEffectRed;
                    self.enterOverlayMaterial = matMercEnergizedRed;
                    self.orbEffect = MercFocusedAssaultOrbEffectRed;
                }
                orig(self);
            };

            //Maybe hook into huntress as well?
            //Done to prevent wrong colorations
            On.EntityStates.Huntress.BlinkState.CreateBlinkEffect += (orig, self, origin) =>
            {
                matHuntressFlashBright.SetColor("_TintColor", new Color(0.0191f, 1.1386f, 1.2973f, 1));//0.0191 1.1386 1.2973 1
                matHuntressFlashExpanded.SetColor("_TintColor", new Color(0f, 0.4367f, 0.5809f, 1));//0 0.4367 0.5809 1
                orig(self, origin);
            };

            On.EntityStates.Huntress.BaseBeginArrowBarrage.CreateBlinkEffect += (orig, self, origin) =>
            {
                matHuntressFlashBright.SetColor("_TintColor", new Color(0.0191f, 1.1386f, 1.2973f, 1));//0.0191 1.1386 1.2973 1
                matHuntressFlashExpanded.SetColor("_TintColor", new Color(0f, 0.4367f, 0.5809f, 1));//0 0.4367 0.5809 1
                orig(self, origin);
            };

            //Needs to be done like this due to EvisDash.FixedUpdate() using LegacyLoad on the Material can't be replaced without IL
            On.EntityStates.Merc.EvisDash.CreateBlinkEffect += (orig, self, origin) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex == 1)
                {
                    EntityStates.Merc.EvisDash.blinkPrefab = HuntressBlinkEffectRed;
                    matHuntressFlashBright.SetColor("_TintColor", new Color(1.3f, 0.6f, 0.6f, 1f));//0.0191 1.1386 1.2973 1
                    matHuntressFlashExpanded.SetColor("_TintColor", new Color(0.58f, 0.2f, 0.2f, 1f));//0 0.4367 0.5809 1
                    orig(self, origin);
                    return;
                }
                else
                {
                    matHuntressFlashBright.SetColor("_TintColor", new Color(0.0191f, 1.1386f, 1.2973f, 1));//0.0191 1.1386 1.2973 1
                    matHuntressFlashExpanded.SetColor("_TintColor", new Color(0f, 0.4367f, 0.5809f, 1));//0 0.4367 0.5809 1
                }
                EntityStates.Merc.EvisDash.blinkPrefab = HuntressBlinkEffect;
                orig(self, origin);
            };
            On.EntityStates.Merc.Evis.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex != MercSkinSpecial)
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        EntityStates.Merc.Evis.blinkPrefab = HuntressFireArrowRainRed;
                        EntityStates.Merc.Evis.hitEffectPrefab = OmniImpactVFXSlashMercEvisRed;
                    }
                    else
                    {
                        EntityStates.Merc.Evis.blinkPrefab = HuntressFireArrowRain;
                        EntityStates.Merc.Evis.hitEffectPrefab = OmniImpactVFXSlashMercEvis;
                    }
                }
                orig(self);
            };

            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += (orig, self) =>
            {
                if (self.outer.commonComponents.characterBody.skinIndex == 1)
                {
                    self.effectPrefab = MercSwordFinisherSlashRed;
                    self.projectilePrefab = EvisProjectileRed; //Replace Ghost Prefab not actual projectile
                }
                orig(self);
            };
        }
        */

        public static void ChangeLights()
        {
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody body)
        {
            if (body.name == "MercBody(Clone)")
            {
                // Main.InfernoLogger.LogError("body skin index is " + body.skinIndex);
                // Main.InfernoLogger.LogError("SkinCatalog.FindLocalSkinIndexForBody(body.bodyIndex, InfernoSkinMod.InfernoSkinModPlugin.MercenarySkin) is " + SkinCatalog.FindLocalSkinIndexForBody(body.bodyIndex, InfernoSkinMod.InfernoSkinModPlugin.MercenarySkin));
                if (SkinCatalog.FindLocalSkinIndexForBody(body.bodyIndex, InfernoSkinMod.InfernoSkinModPlugin.MercenarySkin) == body.skinIndex)
                {
                    // Main.InfernoLogger.LogError("body skin index is equal to inferno mercenary skin");
                    var modelLocator = body.GetComponent<ModelLocator>();
                    if (modelLocator)
                    {
                        // Main.InfernoLogger.LogError("modelLocator found");
                        var model = modelLocator.modelTransform;
                        if (model)
                        {
                            // Main.InfernoLogger.LogError("modelTransform found");
                            var lightInfos = model.GetComponent<CharacterModel>().baseLightInfos;

                            var backLight = lightInfos[0].light;
                            lightInfos[0].defaultColor = new Color32(38, 33, 176, 255);
                            // var darkBlue = new Color(0.03529411765f, 0f, 1f, 1f);
                            // backLight.set_color_Injected(ref darkBlue);
                            backLight.color = new Color32(38, 33, 176, 255);
                            backLight.intensity = 2f;
                            backLight.range = 5f;

                            backLight.gameObject.GetComponent<FlickerLight>().enabled = false;

                            var swordLight = lightInfos[1].light;
                            lightInfos[1].defaultColor = new Color32(255, 175, 0, 255);
                            // var pissYellow = new Color(1f, 0.6862745098f, 0f, 1f);
                            // swordLight.set_color_Injected(ref pissYellow);
                            swordLight.color = new Color32(255, 175, 0, 255);
                            swordLight.intensity = 1.5f;
                            swordLight.range = 2f;

                            swordLight.gameObject.GetComponent<FlickerLight>().enabled = false;
                        }
                    }
                }
            }
        }
    }
}

// TODO:
// SWUFF HELP AAAA