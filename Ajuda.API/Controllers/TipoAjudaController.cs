using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ajuda.API.Controllers
{
    /// <summary>
    /// Controlador para consultar os tipos de ajuda disponíveis.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "2 - TipoAjuda")]
    public class TipoAjudaController : ControllerBase
    {
        private readonly ITipoAjudaService _service;

        public TipoAjudaController(ITipoAjudaService service)
        {
            _service = service;
        }

        /// <summary>Lista todos os tipos de ajuda disponíveis.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TipoAjuda>), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tipos = await _service.ListarAsync();
                return Ok(tipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar tipos de ajuda: {ex.Message}");
            }
        }

        /// <summary>Retorna um tipo de ajuda pelo ID.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TipoAjuda), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var tipo = await _service.ObterPorIdAsync(id);
                if (tipo == null)
                    return NotFound("Tipo de ajuda não encontrado.");

                return Ok(tipo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar tipo de ajuda: {ex.Message}");
            }
        }
    }
}
