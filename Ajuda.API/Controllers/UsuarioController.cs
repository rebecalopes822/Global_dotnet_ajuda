using Ajuda.API.DTOs;
using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ajuda.API.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações com usuários.
    /// </summary>
    [ApiExplorerSettings(GroupName = "1 - Usuário")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os usuários cadastrados.
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Usuario>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarios = await _service.ListarAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar usuários: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Usuário encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var usuario = await _service.ObterPorIdAsync(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="dto">Dados do usuário</param>
        /// <returns>Usuário criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] UsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.EhVoluntario != 0 && dto.EhVoluntario != 1)
                return BadRequest("Campo EhVoluntario inválido. Use 1 para Sim ou 0 para Não.");

            try
            {
                var usuario = new Usuario
                {
                    Nome = dto.Nome,
                    Email = dto.Email,
                    Telefone = dto.Telefone,
                    EhVoluntario = dto.EhVoluntario
                };

                var novo = await _service.CriarAsync(usuario);
                return CreatedAtAction(nameof(Get), new { id = novo.Id }, novo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="dto">Dados atualizados</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID do corpo e da URL devem ser iguais.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.EhVoluntario != 0 && dto.EhVoluntario != 1)
                return BadRequest("Campo EhVoluntario inválido. Use 1 para Sim ou 0 para Não.");

            try
            {
                var existente = await _service.ObterPorIdAsync(id);
                if (existente == null)
                    return NotFound("Usuário não encontrado.");

                existente.Nome = dto.Nome;
                existente.Email = dto.Email;
                existente.Telefone = dto.Telefone;
                existente.EhVoluntario = dto.EhVoluntario;

                await _service.AtualizarAsync(existente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Deleta um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existente = await _service.ObterPorIdAsync(id);
                if (existente == null)
                    return NotFound("Usuário não encontrado.");

                await _service.DeletarAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar usuário: {ex.Message}");
            }
        }
    }
}
