using System.Collections.Generic;
using System.Drawing;

namespace ml.ImageBlurrer.Shared
{
    public class PositionColor
    {
        public PositionColor(Point point, Color color)
        {
            Point = point;
            Color = color;
        }
        
        public Point Point { get; set; }
        public Color Color { get; set; }
    }
}