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
    [HarmonyPatch(typeof(FloatMenuOption), "DoGUI")]
    class PatchFloatMenuOptionDoGUI
    {
        static RecipeTooltip tooltip = new RecipeTooltip();

        static void Postfix(FloatMenuOption __instance, Rect rect, bool colonistOrdering, FloatMenu floatMenu)
        {
            if (!RecipeIcons.settings.enableTooltip) return;
            if (!Mouse.IsOver(rect.TopPartPixels(rect.height - 1))) return;

            tooltip.ShowAt(
                __instance,
                Find.WindowStack.currentlyDrawnWindow.windowRect.x + rect.x + rect.width + 5,
                Find.WindowStack.currentlyDrawnWindow.windowRect.y + rect.y
            );

        }


    }
}
