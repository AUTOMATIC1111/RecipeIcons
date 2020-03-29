using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Verse;

namespace RecipeIcons
{
    public class RecipeIcons : Mod
    {
        public static Settings settings;

        public RecipeIcons(ModContentPack pack) : base(pack)
        {
            var harmony = new Harmony("com.github.automatic1111.recipeicons");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "RecipeIconsTitle".Translate();
        }
    }
}
