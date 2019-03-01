using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace RecipeIcons
{
    [HarmonyPatch(typeof(Widgets), "InfoCardButton", new Type[] { typeof(float), typeof(float), typeof(Def) }), StaticConstructorOnStartup]
    class PatchWidgetsInfoCardButton
    {
        static bool Prefix(float x, float y, Def def)
        {
            Texture2D icon = null;
            Color color = GUI.color;

            RecipeDef recipe = def as RecipeDef;
            if (recipe != null) {
                if (recipe.products.Count == 0) return true;
                if (recipe.products[0].thingDef == null) return true;

                icon = recipe.products[0].thingDef.uiIcon;
                color = recipe.products[0].thingDef.uiIconColor;
            }

            ThingDef thing = def as ThingDef;
            if (thing != null)
            {
                icon = thing.uiIcon;
                color = thing.uiIconColor;
            }

            if (icon == null) return true;

            Rect rect = new Rect(x, y, 24f, 24f);
            if (Widgets.ButtonImage(rect, icon, color))
            {
                Find.WindowStack.Add(new Dialog_InfoCard(def));
                return true;
            }

            TooltipHandler.TipRegion(rect, "DefInfoTip".Translate());
            UIHighlighter.HighlightOpportunity(rect, "InfoCard");
            return false;
        }
    }

    
}