using System;
using System.Drawing;

namespace sf
{
    internal class RealTimeImageMaker
    {
        private Color backColor;
        private Color blue;
        private int height;
        private int width;

        public RealTimeImageMaker(int width, int height, Color backColor, Color blue)
        {
            this.width = width;
            this.height = height;
            this.backColor = backColor;
            this.blue = blue;
        }

        internal Image GetCurrentCurve()
        {
            throw new NotImplementedException();
        }
    }
}