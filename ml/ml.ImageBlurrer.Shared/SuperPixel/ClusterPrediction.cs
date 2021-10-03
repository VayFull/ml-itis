using Microsoft.ML.Data;

namespace ml.ImageBlurrer.Shared
{
    /// <summary>
    /// Выходная модель предсказания для ml.net
    /// </summary>
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}