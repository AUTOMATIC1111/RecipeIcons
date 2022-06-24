using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RecipeIcons
{
    public class Settings : ModSettings
    {
        public bool enableTooltip = true;
        public bool disableSmallMenus = true;

        public bool showMod = true;

        override public void ExposeData()
        {
            Scribe_Values.Look(ref enableTooltip, "enableTooltip");
            Scribe_Values.Look(ref disableSmallMenus, "disableSmallMenus");
            Scribe_Values.Look(ref showMod, "showMod");
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("RecipeIconsEnableTooltipName".Translate(), ref enableTooltip, "RecipeIconsEnableTooltipDesc".Translate());
            listing_Standard.CheckboxLabeled("RecipeIconsDisableSmallMenusName".Translate(), ref disableSmallMenus, "RecipeIconsDisableSmallMenusDesc".Translate());
            listing_Standard.CheckboxLabeled("RecipeIconsShowModName".Translate(), ref showMod, "RecipeIconsShowModDesc".Translate());
            listing_Standard.End();
        }
    }

}
