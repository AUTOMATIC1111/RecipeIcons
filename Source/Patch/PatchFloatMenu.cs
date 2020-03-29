using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RecipeIcons.Patch
{
    [HarmonyPatch(typeof(FloatMenu), "get_SizeMode")]
    class PatchFloatMenuFloatMenuSizeMode
    {
        static FloatMenuSizeMode Postfix(FloatMenuSizeMode value)
        {
            if (RecipeIcons.settings.disableSmallMenus) return FloatMenuSizeMode.Normal;

            return value;
        }
    }
}
