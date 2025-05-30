using Microsoft.ML.Data;

namespace Ajuda.ML
{
    public class UrgenciaModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string NivelUrgencia { get; set; } = string.Empty;

        public float[] Score { get; set; }
    }
}
