using Ajuda.API.DTOs;
using Ajuda.API.Models;
using Ajuda.API.Mensageria;
using Microsoft.AspNetCore.Mvc;
using Ajuda.API.Services.Interfaces;

namespace Ajuda.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "3 - PedidoAjuda")]
    public class PedidoAjudaController : ControllerBase
    {
        private readonly IPedidoAjudaService _service;
        private readonly PedidoAjudaQueue _fila;

        public PedidoAjudaController(IPedidoAjudaService service, PedidoAjudaQueue fila)
        {
            _service = service;
            _fila = fila;
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

        /// <summary>Cria um novo pedido de ajuda e envia para fila assíncrona.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(PedidoAjuda), 202)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] PedidoAjudaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var entidade = await _service.MapearParaEntidade(dto);

                await _fila.EnqueueAsync(entidade);

                return Accepted("Pedido enfileirado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enfileirar pedido: {ex.Message}");
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
                return StatusCode(500, $"Erro ao atualizar pedido: {ex.Message}");
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
