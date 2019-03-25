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
            public Texture2D texture2D = null;
            public ThingDef thingDef;
            public Color textureColor;
            public string tooltip;

            public Icon(ThingDef thing, string tooltip)
            {
                thingDef = thing;

                material = thing != null && thing.graphic != null && thing.graphicData != null ? thing.graphic.MatSingle : null;
                if (material != null)
                {
                    if (thing.graphicData.shaderType == ShaderTypeDefOf.CutoutComplex)
                    {
                        texture = material.mainTexture;
                    }
                    else
                    {
                        texture2D = thing.uiIcon;
                        textureColor = thing.uiIconColor;
                        material = null;
                    }
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
                    return new Icon(ing.FixedIngredient, null);
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

            Rect rect = new Rect(x-2, y-2, 28f, 28f);

            bool clicked;
            if (icon.texture2D != null)
            {
                clicked = Widgets.ButtonImage(rect, icon.texture2D, icon.textureColor);
            }
            else if (icon.material != null)
            {
                GenUI.DrawTextureWithMaterial(rect, icon.texture, icon.material);
                clicked = Widgets.ButtonInvisible(rect);
            }
            else
            {
                return true;
            }

            if (clicked)
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