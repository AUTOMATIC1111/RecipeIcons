using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RecipeIcons
{
    public class TextLayout
    {
        static Rect rect = default;

        public float lineHeight;
        public float x;
        public float y;
        public float startX;
        public float padding;

        bool measuring;


        private float calculatedWidth;
        private float calculatedHeight;
        public float Width => calculatedWidth + padding;
        public float Height => calculatedHeight+ lineHeight + padding;

        public void StartMeasuring()
        {
            measuring = true;
            calculatedWidth = 0;
            calculatedHeight = 0;
            x = padding;
            y = padding;
        }

        public void StartDrawing(float x, float y)
        {
            measuring = false;
            startX = this.x = x + padding;
            this.y = y + padding;
        }

        public void Text(string text)
        {
            Vector2 size = Verse.Text.CalcSize(text);

            if (!measuring) {
                rect.x = x;
                rect.y = y;
                rect.width = size.x;
                rect.height = size.y;
                Widgets.Label(rect, text);
            }

            x += size.x;
            if (x > calculatedWidth) calculatedWidth = x;
            if (y > calculatedHeight) calculatedHeight = y;
        }

       
        public void Icon(Icon icon, int width)
        {
            if (!measuring)
            {
                rect.x = x;
                rect.y = y + (lineHeight - width) / 2 - 1;
                rect.width = width;
                rect.height = width;
                icon.Draw(rect);
            }

            x += width;
            if (x > calculatedWidth) calculatedWidth = x;
            if (y > calculatedHeight) calculatedHeight = y;
        }

        public void Newline() {
            x = startX;
            y += lineHeight;
        }
    }
}
