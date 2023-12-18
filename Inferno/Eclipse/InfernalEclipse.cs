using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using static R2API.LanguageAPI;

namespace Inferno.Eclipse
{
    public static class InfernalEclipse
    {
        public static List<string> cachedDescriptions = new();

        public static void Init()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        public static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var sceneName = scene.name;
            if (sceneName == "eclipseworld")
            {
                Language.SetCurrentLanguage(Language.currentLanguageName);
                for (int i = 0; i < 16; i++)
                {
                    var j = i + 1;
                    var token = "ECLIPSE_" + j + "_DESCRIPTION";
                    /*
                    if (cachedDescriptions.Count < 16)
                    {
                        cachedDescriptions.Add(Language.GetString(token));
                        Main.InfernoLogger.LogError("adding description to cache, i is " + i + ", j is " + j);
                    }
                    */
                    LanguageOverlay languageOverlay = null;

                    if (Main.InfernalEclipse.Value)
                    {
                        // Main.InfernoLogger.LogError("adding infernal descriptions ");
                        languageOverlay = LanguageAPI.AddOverlay(token, Language.GetString(token).Replace("Monsoon", "<style=cDeath>Inferno</style>").Replace("\"You only celebrate in the light... because I allow it.\"", "<style=cDeath>\"You'll never bow to another god.</style>\"").Replace("<style=cIsHealth", "<style=cDeath"));
                    }
                    else
                    {
                        if (languageOverlay != null)
                            languageOverlay.Remove();
                        /*
                        Main.InfernoLogger.LogError("adding cached descriptions back, cached descriptions index i is " + cachedDescriptions[i]);
                        LanguageAPI.AddOverlay(token, cachedDescriptions[i]);
                        */
                    }
                }
                // dont care about these rn cause the stack trace says it throws the thing that works so lmao

                // InfernoLogger.LogError("eclipse world");
                var menu = scene.GetRootGameObjects()[5];
                var weatherEclipse = scene.GetRootGameObjects()[4];
                if (weatherEclipse.name == "Weather, Eclipse")
                {
                    var pp = weatherEclipse.transform.Find("PP + Amb");
                    if (pp)
                    {
                        var postProcessVolume = pp.GetComponent<PostProcessVolume>();
                        if (postProcessVolume)
                        {
                            var profile = postProcessVolume.profile;
                            var colorGrading = profile.GetSetting<ColorGrading>();
                            if (colorGrading)
                            {
                                if (Main.InfernalEclipse.Value)
                                {
                                    colorGrading.saturation.value = 50f;
                                    colorGrading.hueShift.overrideState = true;
                                    colorGrading.hueShift.value = 143f;
                                }
                                else
                                {
                                    colorGrading.saturation.value = -23f;
                                    colorGrading.hueShift.value = 0f;
                                }
                            }
                        }
                    }
                }
                // InfernoLogger.LogError("menu is " + menu);
                if (menu.name == "MENU")
                {
                    // InfernoLogger.LogError("menu is menu yes");
                    var trans = menu.transform;
                    // InfernoLogger.LogError("menu transform is " + trans);
                    var eclipseRunMenu = trans.Find("EclipseRunMenu");
                    // InfernoLogger.LogError("eclipse run menu is " + eclipseRunMenu);
                    var mainPanel = eclipseRunMenu.Find("Main Panel");
                    // InfernoLogger.LogError("main panel is " + mainPanel);
                    var rightPanel = mainPanel.Find("RightPanel");
                    // InfernoLogger.LogError("right panel is " + rightPanel);
                    var medalPanel = rightPanel.Find("MedalPanel");
                    // InfernoLogger.LogError("medal panel is " + medalPanel);
                    var horizontalLayout = medalPanel.Find("HorizontalLayout");
                    // InfernoLogger.LogError("horizontal layout is " + horizontalLayout);
                    for (int i = 0; i < horizontalLayout.childCount; i++)
                    {
                        var child = horizontalLayout.GetChild(i);
                        // InfernoLogger.LogError("child is " + child);
                        var eclipseDifficultyMedalDisplay = child.GetComponent<EclipseDifficultyMedalDisplay>();
                        // InfernoLogger.LogError("eclipseDifficultyMedalDisplay is " + eclipseDifficultyMedalDisplay);
                        if (eclipseDifficultyMedalDisplay)
                        {
                            var incomplete = "texDifficultyEclipse" + (i + 1) + "Icon.png";
                            var complete = "texDifficultyEclipse" + (i + 1) + "IconGold.png";
                            if (Main.InfernalEclipse.Value)
                            {
                                eclipseDifficultyMedalDisplay.incompleteSprite = Main.inferno.LoadAsset<Sprite>("Assets/Inferno/" + incomplete);
                                eclipseDifficultyMedalDisplay.completeSprite = Main.inferno.LoadAsset<Sprite>("Assets/Inferno/" + complete);
                            }
                            else
                            {
                                eclipseDifficultyMedalDisplay.incompleteSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/EclipseRun/" + incomplete).WaitForCompletion();
                                eclipseDifficultyMedalDisplay.completeSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/EclipseRun/" + complete).WaitForCompletion();
                            }
                            eclipseDifficultyMedalDisplay.enabled = false;
                            eclipseDifficultyMedalDisplay.enabled = true; // refresh
                        }
                    }
                }
            }
        }
    }
}