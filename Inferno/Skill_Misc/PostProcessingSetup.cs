using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using RoR2;

namespace Inferno.Skill_Misc
{
    public static class PostProcessingSetup
    {
        public static void Gupdate()
        {
            Main.ppHolder = new("PPInferno");
            Object.DontDestroyOnLoad(Main.ppHolder);
            Main.ppHolder.layer = LayerIndex.postProcess.intVal;
            Main.ppHolder.AddComponent<InfernoPostProcessingController>();
            PostProcessVolume pp = Main.ppHolder.AddComponent<PostProcessVolume>();
            Object.DontDestroyOnLoad(pp);
            pp.isGlobal = true;
            pp.weight = 1f;
            pp.priority = 49;
            PostProcessProfile ppProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
            Object.DontDestroyOnLoad(ppProfile);
            ppProfile.name = "ppInferno";
            Main.cg = ppProfile.AddSettings<ColorGrading>();
            Main.cg.SetAllOverridesTo(true);
            Main.vn = ppProfile.AddSettings<Vignette>();
            Main.vn.SetAllOverridesTo(true);

            pp.sharedProfile = ppProfile;
        }
    }
    public class InfernoPostProcessingController : MonoBehaviour
    {
        public PostProcessVolume volume;

        public void Start()
        {
            volume = GetComponent<PostProcessVolume>();
        }
    }
}
