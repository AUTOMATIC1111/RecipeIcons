using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RecipeIcons
{
    class Icon
    {
        public static Icon missing = new Icon(null);

        public Material material = null;
        public Texture2D texture2D = null;
        public Texture texture = null;
        public ThingDef thingDef;
        public Color textureColor;

        public Icon(ThingDef thing)
        {
            thingDef = thing;

            if (thing != null)
            {
                texture2D = thing.uiIcon == BaseContent.BadTex ? null : thing.uiIcon;
                textureColor = thing.uiIconColor;

                if (thing.graphic != null && thing.graphicData != null && thing.graphicData.shaderType == ShaderTypeDefOf.CutoutComplex && ! thing.graphic.MatSingle.NullOrBad())
                {
                    material = thing.graphic.MatSingle;
                    texture = material.mainTexture;
                }
            }

        }

        public bool Draw(Rect rect)
        {
            if (material != null)
            {
                if (texture.width != texture.height && rect.width == rect.height)
                {
                    float ratio = texture.width / texture.height;
                    if (ratio < 1) { rect.x += (rect.width - rect.width * ratio) / 2; rect.width *= ratio; }
                    else { rect.y += (rect.height - rect.height / ratio) / 2; rect.height /= ratio; };
                }

                Graphics.DrawTexture(rect, texture, material, 0);
                return true;
            }

            return Draw2D(rect);
        }

        public bool Draw2D(Rect rect)
        {
            if (texture2D != null)
            {
                if (texture2D.width != texture2D.height && rect.width == rect.height)
                {
                    float ratio = texture2D.width / texture2D.height;
                    if (ratio < 1) { rect.x += (rect.width - rect.width * ratio) / 2; rect.width *= ratio; }
                    else { rect.y += (rect.height - rect.height / ratio) / 2; rect.height /= ratio; };
                }

                GUI.color = new Color(textureColor.r, textureColor.g, textureColor.b, GUI.color.a);
                GUI.DrawTexture(rect, texture2D);
                return true;
            }

            return false;
        }

        static Dictionary<Def, Icon> map = new Dictionary<Def, Icon>();
        public static Icon getIcon(Def def)
        {
            if (def == null) return missing;

            Icon res;

            if (map.TryGetValue(def, out res)) return res;

            res = getIconWithoutCache(def);
            map.Add(def, res);
            return res;
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

                        return new Icon(item.thingDef);
                    }
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

            return missing;
        }

    }
}
