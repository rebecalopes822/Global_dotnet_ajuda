using Microsoft.ML.Data;

namespace Ajuda.ML
{
    public class UrgenciaModelInput
    {
        [LoadColumn(0)]
        public float TipoAjudaId { get; set; }

        [LoadColumn(1)]
        public float Idade { get; set; }

        [LoadColumn(2)]
        public float CriancasNoLocal { get; set; }

        [LoadColumn(3)]
        public float PessoasNoLocal { get; set; }

        [LoadColumn(4)]
        public float SituacaoDeRisco { get; set; }

        [LoadColumn(5)]
        public string NivelUrgencia { get; set; } = string.Empty;
    }
}
