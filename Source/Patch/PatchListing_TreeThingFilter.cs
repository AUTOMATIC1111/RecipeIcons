using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RecipeIcons.Patch
{
    [HarmonyPatch(typeof(Listing_TreeThingFilter), "DoThingDef")]
    class PatchListing_TreeThingFilter
    {
        static void Prefix(Listing_TreeThingFilter __instance, float ___curY, ThingDef tDef, ref int nestLevel)
        {
            nestLevel++;

            Icon icon = Icon.getIcon(tDef);
            if (icon == Icon.missing) return;

            Rect rect = new Rect(__instance.nestIndentWidth * nestLevel - 6f, ___curY, 20f, 20f);
            icon.Draw2D(rect);
            GUI.color = Color.white;
        }
    }
}
