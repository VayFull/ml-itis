using System;
using System.Drawing;
using Microsoft.ML.Data;

namespace ml.ImageBlurrer.Shared
{
    /// <summary>
    /// Входная модель для ml.net
    /// </summary>
    public class ColorPositionModel
    {
        public ColorPositionModel(int red, int green, int blue, int xPosition, int yPosition)
        {
            Red = red;
            Green = green;
            Blue = blue;
            XPosition = xPosition;
            YPosition = yPosition;
        }
        [LoadColumn(0)]
        public float Red { get; set; }
        [LoadColumn(1)]
        public float Green { get; set; }
        [LoadColumn(2)]
        public float Blue { get; set; }
        [LoadColumn(3)]
        public float XPosition { get; set; }
        [LoadColumn(4)]
        public float YPosition { get; set; }
    }
}