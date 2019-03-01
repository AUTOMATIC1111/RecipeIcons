using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RecipeIcons
{
    class FloatMenuOptionLeft : FloatMenuOption
    {
        static FieldInfo fieldSizeMode = typeof(FloatMenuOption).GetField("sizeMode", BindingFlags.NonPublic | BindingFlags.Instance);

        private FloatMenuSizeMode sizeMode { get { return (FloatMenuSizeMode) fieldSizeMode.GetValue(this); } }

        static FloatMenuOptionLeft()
        {
            ColorBGActive = new ColorInt(21, 25, 29).ToColor;
            ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;
            ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;
            ColorTextActive = Color.white;
            ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);
        }

        private static readonly Color ColorBGActive;
        private static readonly Color ColorBGActiveMouseover;
        private static readonly Color ColorBGDisabled;
        private static readonly Color ColorTextActive;
        private static readonly Color ColorTextDisabled;

        public FloatMenuOptionLeft(FloatMenuOption item) : base(item.Label, item.action, item.Priority, item.mouseoverGuiAction, item.revalidateClickTarget, item.extraPartWidth, item.extraPartOnGUI, item.revalidateWorldClickTarget)
        {

        }
        
        private float VerticalMargin
        {
            get
            {
                return (this.sizeMode != FloatMenuSizeMode.Normal) ? 1f : 4f;
            }
        }
        
        private float HorizontalMargin
        {
            get
            {
                return (this.sizeMode != FloatMenuSizeMode.Normal) ? 3f : 6f;
            }
        }
        
        private GameFont CurrentFont
        {
            get
            {
                return (this.sizeMode != FloatMenuSizeMode.Normal) ? GameFont.Tiny : GameFont.Small;
            }
        }

        public override bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu) {
            Rect rect2 = rect;
            rect2.height -= 1f;
            bool flag = !this.Disabled && Mouse.IsOver(rect2);
            bool flag2 = false;
            Text.Font = this.CurrentFont;
            Rect rect3 = rect;
            rect3.xMin += this.HorizontalMargin;
            rect3.xMin += this.extraPartWidth;
            rect3.xMax -= this.HorizontalMargin;
            rect3.xMax -= 4f;
            if (flag)
            {
                rect3.x += 4f;
            }
            Rect rect4 = default(Rect);
            if (this.extraPartWidth != 0f)
            {
                rect4 = new Rect(rect.xMin, rect.yMin, this.extraPartWidth, 30f);
                flag2 = Mouse.IsOver(rect4);
            }
            if (!this.Disabled)
            {
                MouseoverSounds.DoRegion(rect2);
            }
            Color color = GUI.color;
            if (this.Disabled)
            {
                GUI.color = ColorBGDisabled * color;
            }
            else if (flag && !flag2)
            {
                GUI.color = ColorBGActiveMouseover * color;
            }
            else
            {
                GUI.color = ColorBGActive * color;
            }
            GUI.DrawTexture(rect, BaseContent.WhiteTex);
            GUI.color = (this.Disabled ? ColorTextDisabled : ColorTextActive) * color;
            if (this.sizeMode == FloatMenuSizeMode.Tiny)
            {
                rect3.y += 1f;
            }
            Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect3, this.Label);
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = color;
            if (this.extraPartOnGUI != null)
            {
                bool flag3 = this.extraPartOnGUI(rect4);
                GUI.color = color;
                if (flag3)
                {
                    return true;
                }
            }
            if (flag && this.mouseoverGuiAction != null)
            {
                this.mouseoverGuiAction();
            }
            if (this.tutorTag != null)
            {
                UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
            }
            if (!Widgets.ButtonInvisible(rect2, false))
            {
                return false;
            }
            if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
            {
                return false;
            }
            this.Chosen(colonistOrdering, floatMenu);
            if (this.tutorTag != null)
            {
                TutorSystem.Notify_Event(this.tutorTag);
            }
            return true;
        }
    }
}
