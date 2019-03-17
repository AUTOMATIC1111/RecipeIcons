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
    [HarmonyPatch(typeof(Widgets), "InfoCardButton", new Type[] { typeof(float), typeof(float), typeof(Def) })]
    class PatchWidgetsInfoCardButton
    {
        class Icon
        {
            public ThingDef thingDef = null;
            public Graphic graphic = null;

            public Icon(ThingDef thing)
            {
                thingDef = thing;
                graphic = thing.graphic;
            }
        }

        static Dictionary<Def, Icon> map = new Dictionary<Def, Icon>();
        static Icon getIcon(Def def)
        {
            Icon res;

            if (map.TryGetValue(def, out res)) return res;

            res = getIconWithoutCache(def);
            if (res == null) return null;

            map.Add(def, res);
            return res;
        }

        static Icon getIconWithoutCache(Def def)
        {
            RecipeDef recipe = def as RecipeDef;
            if (recipe != null)
            {
                if (recipe.products.Count != 0)
                {
                    if (recipe.products[0].thingDef == null) return null;

                    return new Icon(recipe.products[0].thingDef);
                }

                foreach (IngredientCount ing in recipe.ingredients)
                {
                    if (ing == null) continue;
                    if (!ing.IsFixedIngredient) continue;
                    return new Icon(ing.FixedIngredient);
                }
            }

            ThingDef thing = def as ThingDef;
            if (thing != null)
            {
                return new Icon(thing);
            }

            return null;
        }
        
        static bool Prefix(float x, float y, Def def)
        {
            Icon icon = getIcon(def);
            if (icon == null) return true;

            Rect rect = new Rect(x-2, y-2, 28f, 28f);
            Graphics.DrawTexture(rect, icon.graphic.MatSingle.mainTexture, icon.graphic.MatSingle);
            if (Widgets.ButtonInvisible(rect))
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