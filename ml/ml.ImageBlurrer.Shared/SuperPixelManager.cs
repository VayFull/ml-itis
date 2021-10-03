using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace ml.ImageBlurrer.Shared
{
    public class SuperPixelManager
    {
        public void GetImageAndSaveSuperPixelized(string imageName, string editedImageName, string imageDirectory,
            float coef, int numberOfClusters)
        {
            var imagePath = $"{imageDirectory}{imageName}";
            var editedImagePath = $"{imageDirectory}{editedImageName}";

            var image = new Bitmap(imagePath);

            var editedImage = GetSuperPixelizedImageOf(image, coef, numberOfClusters);

            editedImage.Save(editedImagePath);
        }

        public Bitmap GetSuperPixelizedImageOf(Bitmap initialImage, float positionImportance, int numberOfClusters)
        {
            var pixelsDictionary = new Dictionary<Color, int>();

            for (int x = 0; x < initialImage.Width; x++)
            {
                for (int y = 0; y < initialImage.Height; y++)
                {
                    var pixel = initialImage.GetPixel(x, y);
                    pixelsDictionary.TryGetValue(pixel, out var currentCount);
                    pixelsDictionary[pixel] = currentCount + 1;
                }
            }

            var colorPositionList = new List<ColorPositionModel>();
            var transformedColorPisitionDictionary = new Dictionary<ColorPositionModel, PositionColor>();

            for (int x = 0; x < initialImage.Width; x++)
            {
                for (int y = 0; y < initialImage.Height; y++)
                {
                    var pixel = initialImage.GetPixel(x, y);
                    var colorPositionModel = new ColorPositionModel(pixel.R, pixel.G, pixel.B, 
                        (int) (x * positionImportance), (int) (y * positionImportance));
                    colorPositionList.Add(colorPositionModel);

                    transformedColorPisitionDictionary[colorPositionModel] = new PositionColor(new Point(x, y),
                        Color.FromArgb(255, pixel.R, pixel.G, pixel.B));
                }
            }

            var context = new MLContext();

            var dataView = context.Data.LoadFromEnumerable(colorPositionList);

            string featuresColumnName = "Features";

            var pipeline = context.Transforms
                .Concatenate(featuresColumnName, "Red", "Green", "Blue", "XPosition", "YPosition")
                .Append(context.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: numberOfClusters));

            var model = pipeline.Fit(dataView);

            var transformedModel = model.Transform(dataView);

            var data = context.Data
                .CreateEnumerable<ClusterPrediction>(transformedModel, false)
                .ToList();

            int k;
            var centroids = new VBuffer<float>[] { };
            model.LastTransformer.Model.GetClusterCentroids(ref centroids, out k);

            var mediumList = new Dictionary<uint, List<PositionColor>>();
            for (uint i = 1; i <= numberOfClusters; i++)
            {
                mediumList[i] = new List<PositionColor>();
            }

            for (int i = 0; i < colorPositionList.Count; i++)
            {
                var originalColorPosition = transformedColorPisitionDictionary[colorPositionList[i]];
                
                var color = originalColorPosition.Color;
                var point = originalColorPosition.Point;

                mediumList[data[i].PredictedClusterId].Add(new PositionColor(point, color));
            }

            for (uint i = 1; i <= numberOfClusters; i++)
            {
                var red = mediumList[i].Sum(x => x.Color.R);
                var green = mediumList[i].Sum(x => x.Color.G);
                var blue = mediumList[i].Sum(x => x.Color.B);
                var count = mediumList[i].Count;
                var averageColor = Color.FromArgb(red / count, green / count, blue / count);
                foreach (var pixel in mediumList[i])
                {
                    initialImage.SetPixel(pixel.Point.X, pixel.Point.Y, averageColor);
                }
            }

            /*foreach (var centroid in centroids)
            {
                var updatedCentroid = centroid.Items()
                    .Where(x => x.Key == 3 || x.Key == 4)
                    .Select(x => x.Value)
                    .ToList();

                var point = new Point((int) updatedCentroid[0], (int) updatedCentroid[1]);
                initialImage.SetPixel(point.X, point.Y, Color.Black);
            }*/

            return initialImage;
        }
    }
}