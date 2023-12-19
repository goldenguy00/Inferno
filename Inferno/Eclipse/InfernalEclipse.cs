using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static R2API.LanguageAPI;

namespace Inferno.Eclipse
{
    public static class InfernalEclipse
    {
        public static List<LanguageOverlay> languageOverlays = new();
        public static List<LanguageOverlay> languageOverlaysEE = new();
        public static Sprite cachedDifficultySprite;

        public static void Init()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var sceneName = scene.name;
            if (sceneName == "eclipseworld")
            {
                var pp = scene.GetRootGameObjects()[0];
                if (pp)
                {
                    var postProcessVolume = pp.GetComponent<PostProcessVolume>();
                    postProcessVolume.StartCoroutine(JankAsHell(scene));
                }
            }
        }

        public static IEnumerator JankAsHell(Scene scene)
        {
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 16; i++)
            {
                var j = i + 1;
                var token = "ECLIPSE_" + j + "_DESCRIPTION";
                var e16token = "GROOVYECLIPSE_" + j + "_DESCRIPTION";

                if (Main.InfernalEclipse.Value)
                {
                    languageOverlays.Add(AddOverlay(token, Language.GetString(token).Replace("Monsoon", "<style=cDeath>Inferno</style>").Replace("\"You only celebrate in the light... because I allow it.\"", "<style=cDeath>\"You'll never bow to another god.</style>\"").Replace("<style=cIsHealth", "<style=cDeath")));
                    languageOverlaysEE.Add(AddOverlay(e16token, Language.GetString(e16token).Replace("Monsoon", "<style=cDeath>Inferno</style>").Replace("\"You only celebrate in the light... because I allow it.\"", "<style=cDeath>\"You'll never bow to another god.</style>\"").Replace("<style=cIsHealth", "<style=cDeath")));
                }
                else
                {
                    if (languageOverlays.Count > 0)
                    {
                        languageOverlays[i].Remove();
                    }

                    if (languageOverlaysEE.Count > 0)
                    {
                        languageOverlaysEE[i].Remove();
                    }
                }
            }

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
            if (menu.name == "MENU")
            {
                var trans = menu.transform;
                var eclipseRunMenu = trans.Find("EclipseRunMenu");
                var mainPanel = eclipseRunMenu.Find("Main Panel");
                var rightPanel = mainPanel.Find("RightPanel");
                var medalPanel = rightPanel.Find("MedalPanel");
                var horizontalLayout = medalPanel.Find("HorizontalLayout");
                if (horizontalLayout)
                {
                    SwapIcons(0, horizontalLayout);
                }
                var horizontalLayoutClone = medalPanel.Find("HorizontalLayout(Clone)");
                if (horizontalLayoutClone)
                {
                    SwapIcons(9, horizontalLayoutClone);
                }
            }
            Language.SetCurrentLanguage(Language.currentLanguageName);
            yield return null;
        }

        public static void SwapIcons(int startingIndex, Transform trans)
        {
            var prefix = "Assets/Inferno/";
            var prefix2 = "RoR2/Base/EclipseRun/";
            var e16 = trans.name.Contains("(Clone)");
            for (int i = startingIndex; i < trans.childCount + startingIndex; i++)
            {
                var j = e16 ? i : (i + 1);
                var child = trans.GetChild(i - startingIndex);
                var eclipseDifficultyMedalDisplay = child.GetComponent<EclipseDifficultyMedalDisplay>();
                if (eclipseDifficultyMedalDisplay)
                {
                    var incomplete = "texDifficultyEclipse" + j + "Icon.png";
                    var complete = "texDifficultyEclipse" + j + "IconGold.png";
                    var incompleteVanilla = "texDifficultyEclipse" + (i + 1) + "Icon.png";
                    var completeVanilla = "texDifficultyEclipse" + (i + 1) + "IconGold.png";
                    var e16Incomplete = "texFurryDifficultyEclipse" + j + "Icon.png";
                    var e16Complete = "texFurryDifficultyEclipse" + j + "IconGold.png";

                    if (Main.InfernalEclipse.Value)
                    {
                        eclipseDifficultyMedalDisplay.incompleteSprite = Main.inferno.LoadAsset<Sprite>(prefix + incomplete);
                        eclipseDifficultyMedalDisplay.completeSprite = Main.inferno.LoadAsset<Sprite>(prefix + complete);
                    }
                    else
                    {
                        eclipseDifficultyMedalDisplay.incompleteSprite = e16 ? Main.inferno.LoadAsset<Sprite>(prefix + e16Incomplete) : Addressables.LoadAssetAsync<Sprite>(prefix2 + incompleteVanilla).WaitForCompletion();
                        eclipseDifficultyMedalDisplay.completeSprite = e16 ? Main.inferno.LoadAsset<Sprite>(prefix + e16Complete) : Addressables.LoadAssetAsync<Sprite>(prefix2 + completeVanilla).WaitForCompletion();
                    }
                    eclipseDifficultyMedalDisplay.Refresh();
                }
            }
        }

        public static void CurrentDifficultyIconController_Start(On.RoR2.UI.CurrentDifficultyIconController.orig_Start orig, MonoBehaviour self)
        {
            orig(self);
            if (Run.instance)
            {
                cachedDifficultySprite = self.GetComponent<Image>().sprite;
                var difficultyIndex = Run.instance.selectedDifficulty;
                var difficultyDef = DifficultyCatalog.GetDifficultyDef(difficultyIndex);

                var e16 = difficultyIndex > DifficultyIndex.Eclipse8;

                var eclipseLevel = Regex.Replace(difficultyDef.nameToken, "[^.0-9]", "");
                int.TryParse(eclipseLevel, out var eclipseLevelAsNumber);
                if (e16)
                {
                    eclipseLevelAsNumber += 8;
                }

                Main.InfernoLogger.LogError("eclipse level as number is " + eclipseLevelAsNumber);

                if (Main.InfernalEclipse.Value)
                {
                    int currentEclipseLevel = 0;
                    var readonlyNetworkUsers = NetworkUser.readOnlyInstancesList;
                    for (int i = 0; i < readonlyNetworkUsers.Count; i++)
                    {
                        var networkUser = readonlyNetworkUsers[i];
                        var survivorPreference = networkUser.GetSurvivorPreference();
                        if (survivorPreference)
                        {
                            var networkUserEclipseLevel = EclipseRun.GetNetworkUserSurvivorCompletedEclipseLevel(networkUser, survivorPreference) + 1;
                            currentEclipseLevel = ((currentEclipseLevel > 0) ? Math.Min(currentEclipseLevel, networkUserEclipseLevel) : networkUserEclipseLevel);
                        }
                    }
                    currentEclipseLevel = Math.Min(currentEclipseLevel, Main.EclipseExtendedLoaded ? 16 : EclipseRun.maxEclipseLevel);
                    Main.InfernoLogger.LogError("current eclipse level after everything is " + currentEclipseLevel);

                    var isCompleted = eclipseLevelAsNumber >= currentEclipseLevel;

                    if (isCompleted)
                    {
                        self.GetComponent<Image>().sprite = Main.inferno.LoadAsset<Sprite>("Assets/Inferno/texDifficultyEclipse" + eclipseLevelAsNumber + "IconGold.png");
                    }
                    else
                    {
                        self.GetComponent<Image>().sprite = Main.inferno.LoadAsset<Sprite>("Assets/Inferno/texDifficultyEclipse" + eclipseLevelAsNumber + "Icon.png");
                    }
                }

                if (!Main.InfernalEclipse.Value)
                {
                    if (eclipseLevelAsNumber > 8)
                    {
                        self.GetComponent<Image>().sprite = Main.inferno.LoadAsset<Sprite>("Assets/Inferno/texFurryDifficultyEclipse" + eclipseLevelAsNumber + "Icon.png");
                    }
                    else
                    {
                        self.GetComponent<Image>().sprite = difficultyDef.GetIconSprite();
                    }
                }
            }
        }
    }
}