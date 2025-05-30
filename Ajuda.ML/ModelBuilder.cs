using Microsoft.ML;
using Microsoft.ML.Data;
using System;

namespace Ajuda.ML
{
    public static class ModelBuilder
    {
        public static void TreinarModelo()
        {
            var mlContext = new MLContext();

            Console.WriteLine("Carregando dados...");
            var dados = mlContext.Data.LoadFromTextFile<UrgenciaModelInput>(
                path: "treinamento_urgencia.csv",
                hasHeader: true,
                separatorChar: ',');

            Console.WriteLine("Montando pipeline...");
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(UrgenciaModelInput.NivelUrgencia))
                .Append(mlContext.Transforms.Concatenate("Features",
                    nameof(UrgenciaModelInput.TipoAjudaId),
                    nameof(UrgenciaModelInput.Idade),
                    nameof(UrgenciaModelInput.CriancasNoLocal),
                    nameof(UrgenciaModelInput.PessoasNoLocal),
                    nameof(UrgenciaModelInput.SituacaoDeRisco)))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            Console.WriteLine("Treinando modelo...");
            var modelo = pipeline.Fit(dados);

            Console.WriteLine("Salvando modelo em: modelo_urgencia.zip");
            mlContext.Model.Save(modelo, dados.Schema, "modelo_urgencia.zip");

            Console.WriteLine("✅ Modelo treinado e salvo com sucesso!");
        }
    }
}
