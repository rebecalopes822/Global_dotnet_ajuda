using Microsoft.AspNetCore.Mvc;
using Ajuda_MLTrainer;

namespace Ajuda.API.IA
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3 - Inteligência Artificial")]

    public class IAController : ControllerBase
    {
        /// <summary>
        /// Realiza a previsão do nível de urgência de um pedido de ajuda.
        /// </summary>
        [HttpPost("prever-urgencia")]
        public IActionResult PreverUrgencia([FromBody] UrgenciaInputDTO input)
        {
            if (input == null)
                return BadRequest("Dados inválidos.");

            if (input.TipoAjudaId < 1 || input.TipoAjudaId > 6)
                return BadRequest("Tipo de ajuda inválido. Use IDs entre 1 e 6.");

            if (input.CriancasNoLocal != 0 && input.CriancasNoLocal != 1)
                return BadRequest("O campo 'CriancasNoLocal' deve ser 0 (Não) ou 1 (Sim).");

            if (input.PessoasNoLocal < 1 || input.PessoasNoLocal > 20)
                return BadRequest("Quantidade de pessoas inválida. Informe entre 1 e 20.");

            if (input.DiasSemAjuda < 0 || input.DiasSemAjuda > 30)
                return BadRequest("Dias sem ajuda inválido. Máximo permitido: 30.");

            if (input.VoluntariosProximos < 0 || input.VoluntariosProximos > 10)
                return BadRequest("Número de voluntários inválido. Máximo permitido: 10.");

            var modelInput = new UrgenciaModel.ModelInput
            {
                TipoAjudaId = input.TipoAjudaId,
                CriancasNoLocal = input.CriancasNoLocal,
                PessoasNoLocal = input.PessoasNoLocal,
                DiasSemAjuda = input.DiasSemAjuda,
                VoluntariosProximos = input.VoluntariosProximos
            };

            var resultado = UrgenciaModel.Predict(modelInput);

            return Ok(new
            {
                NivelUrgenciaPrevisto = resultado.PredictedLabel
            });
        }
    }
}
