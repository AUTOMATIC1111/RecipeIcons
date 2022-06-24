using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RecipeIcons.Patch
{
    [HarmonyPatch(typeof(Widgets), "DefIcon", new Type[] { typeof(Rect), typeof(Def), typeof(ThingDef), typeof(float), typeof(ThingStyleDef), typeof(bool), typeof(Color?) })]
    class PatchWidgetsDefIcon
    {
        static bool ShiftIsHeld
        {
            get
            {
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }

        static float targetWidth = 28f;
        static bool Prefix(Rect rect, Def def, ThingDef stuffDef, float scale, bool drawPlaceholder)
        {
            // don't do anything for larger icons in architect UI etc.
            if (rect.width != rect.height || rect.width > targetWidth) return true;
            
            Icon icon = Icon.getIcon(def);
            if (icon == Icon.missing) return true;

            float widthDiff = targetWidth - rect.width;
            rect = new Rect(rect.x - widthDiff / 2, rect.y - widthDiff / 2, targetWidth, targetWidth);

            Color color = GUI.color;
            bool shouldRunOriginalFunc = !icon.Draw(rect);
            GUI.color = color;

            return shouldRunOriginalFunc;
        }
    }
}
