using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RecipeIcons
{
    [HarmonyPatch(typeof(FloatMenu), MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(List<FloatMenuOption>) })]
    class PatchFloatMenu
    {
        public static bool drawIconsOnLeft = false;

        static bool Prefix(ref List<FloatMenuOption> options)
        {
            if (!drawIconsOnLeft) return true;

            List<FloatMenuOption> res = new List<FloatMenuOption>();

            foreach (var item in options)
            {
                res.Add(new FloatMenuOptionLeft(item));
            }

            options = res;
            drawIconsOnLeft = false;

            return true;
        }
    }
}
