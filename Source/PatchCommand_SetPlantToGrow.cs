using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RecipeIcons
{

    [HarmonyPatch(typeof(Command_SetPlantToGrow), "ProcessInput", new Type[] { typeof(Event) })]
    class PatchCommand_SetPlantToGrow
    {
        static bool Prefix()
        {
            PatchFloatMenu.drawIconsOnLeft = true;
            return true;
        }
    }
}
