using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RecipeIcons
{

    [HarmonyPatch(typeof(BillStack), "DoListing")]
    class PatchBillStackDoListing
    {
        static bool Prefix(BillStack __instance, Rect rect, ref Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
        {
            PatchFloatMenu.drawIconsOnLeft = true;
            return true;
        }
    }
}
