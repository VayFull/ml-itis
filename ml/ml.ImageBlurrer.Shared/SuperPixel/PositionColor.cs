using System.Collections.Generic;
using System.Drawing;

namespace ml.ImageBlurrer.Shared
{
    /// <summary>
    /// Модель для удобного взаимодействия цвета и позиции
    /// </summary>
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