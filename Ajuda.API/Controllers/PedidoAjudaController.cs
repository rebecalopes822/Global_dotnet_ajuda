using Ajuda.API.DTOs;
using Ajuda.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ajuda.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3 - PedidoAjuda")]
    public class PedidoAjudaController : ControllerBase
    {
        private readonly IPedidoAjudaService _service;

        public PedidoAjudaController(IPedidoAjudaService service)
        {
            _service = service;
        }

        /// <summary>Lista todos os pedidos de ajuda.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PedidoAjudaDetalhadoDto>), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var pedidos = await _service.ListarTodosAsync();
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar pedidos: {ex.Message}");
            }
        }

        /// <summary>Busca um pedido de ajuda pelo ID.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PedidoAjudaDetalhadoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var pedido = await _service.ObterPorIdAsync(id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            return Ok(pedido);
        }

        /// <summary>Cria um novo pedido de ajuda.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(PedidoAjudaDetalhadoDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] PedidoAjudaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var criado = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = criado.Id }, criado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar pedido: {ex.Message}");
            }
        }

        /// <summary>Atualiza um pedido existente.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(int id, [FromBody] PedidoAjudaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var atualizado = await _service.AtualizarAsync(id, dto);
                if (!atualizado)
                    return NotFound("Pedido não encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar pedido: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        /// <summary>Deleta um pedido de ajuda pelo ID.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletado = await _service.DeletarAsync(id);
                if (!deletado)
                    return NotFound("Pedido não encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar pedido: {ex.Message}");
            }
        }
    }
}
