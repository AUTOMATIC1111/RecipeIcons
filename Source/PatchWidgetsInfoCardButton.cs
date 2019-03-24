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
        class Icon
        {
            public Material material = null;
            public Texture texture = null;
            public ThingDef thingDef = null;
            public string tooltip;

            public Icon(ThingDef thing, string tooltip)
            {
                if(thing!=null)
                {
                    material = thing.graphic.MatSingle;
                    texture = material.mainTexture;
                    thingDef = thing;

                    // workaround for buggy plant display; I can't figure out the underlying cause
                    if (thing.plant != null) material = null;
                }
                else
                {
                    texture = TextureInfo;
                }

                this.tooltip = tooltip == null ? "DefInfoTip".Translate() : tooltip;

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

        static string getRecipeIngridients(RecipeDef recipe)
        {
            StringBuilder sb = new StringBuilder();
            int total = 0;

            sb.AppendLine("Ingridients:");

            foreach (IngredientCount ing in recipe.ingredients)
            {
                if (ing == null) continue;
                float count = ing.GetBaseCount();

                sb.Append(" - ");
                if (ing.IsFixedIngredient)
                {
                    sb.Append(ing.FixedIngredient.LabelCap);
                }
                else
                {
                    List<string> cats = Traverse.Create(ing.filter).Field<List<string>>("categories").Value;
                    if (cats != null && cats.Count > 0) {
                        int index = 0;
                        foreach (string catName in cats)
                        {
                            ThingCategoryDef cat = DefDatabase<ThingCategoryDef>.GetNamed(catName, false);
                            if (cat == null) continue;

                            if (index != 0) sb.Append(", ");
                            sb.Append(cat.LabelCap);
                            index++;
                        }
                    }
                }

                if (count != 1)
                {
                    sb.Append(" (");
                    sb.Append(count.ToString());
                    sb.Append(")");
                }
                sb.AppendLine("");
                total++;
            }

            if (total == 0) {
                sb.AppendLine(" - ?");
            }

            return sb.ToString();
        }

        static bool ShiftIsHeld
        {
            get
            {
                return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            }
        }

        static Icon getIconWithoutCache(Def def)
        {
            RecipeDef recipe = def as RecipeDef;
            if (recipe != null)
            {
                if (recipe.products != null)
                {
                    foreach (ThingDefCountClass item in recipe.products)
                    {
                        if (item.thingDef == null) continue;

                        return new Icon(item.thingDef, null);
                    }
                }

                if (recipe.specialProducts != null)
                {
                    foreach (SpecialProductType type in recipe.specialProducts)
                    {
                        return new Icon(null, null);
                    }
                }

                foreach (IngredientCount ing in recipe.ingredients)
                {
                    if (ing == null) continue;
                    if (!ing.IsFixedIngredient) continue;
                    return new Icon(ing.FixedIngredient, getRecipeIngridients(recipe));
                }
            }

            ThingDef thing = def as ThingDef;
            if (thing != null)
            {
                return new Icon(thing, null);
            }

            return null;
        }
        
        static bool Prefix(float x, float y, Def def)
        {
            Icon icon = getIcon(def);
            if (icon == null) return true;

            Rect rect = icon.texture==TextureInfo ? new Rect(x, y, 24f, 24f) : new Rect(x-2, y-2, 28f, 28f);
            GenUI.DrawTextureWithMaterial(rect, icon.texture, icon.material);

            if (Widgets.ButtonInvisible(rect))
            {
                Find.WindowStack.Add(new Dialog_InfoCard((ShiftIsHeld && icon.thingDef!=null) ? icon.thingDef : def));
                return true;
            }

            TooltipHandler.TipRegion(rect, icon.tooltip);
            UIHighlighter.HighlightOpportunity(rect, "InfoCard");
            return false;
        }



        public static readonly Texture2D TextureInfo = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);
    }
}