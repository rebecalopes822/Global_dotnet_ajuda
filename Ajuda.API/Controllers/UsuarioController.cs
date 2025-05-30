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

        /// <summary>Lista todos os usuários cadastrados com links HATEOAS.</summary>
        [HttpGet(Name = "GetUsuarios")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioComLinksDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarios = await _service.ListarAsync();

                var usuariosComLinks = usuarios.Select(usuario =>
                {
                    var dto = new UsuarioComLinksDto(usuario);
                    dto.Links.Add(new LinkDto(Url.Link("GetUsuarioById", new { id = usuario.Id })!, "self", "GET"));
                    dto.Links.Add(new LinkDto(Url.Link("AtualizarUsuario", new { id = usuario.Id })!, "update", "PUT"));
                    dto.Links.Add(new LinkDto(Url.Link("DeletarUsuario", new { id = usuario.Id })!, "delete", "DELETE"));
                    return dto;
                });

                return Ok(usuariosComLinks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar usuários: {ex.Message}");
            }
        }

        /// <summary>Busca um usuário pelo ID.</summary>
        [HttpGet("{id}", Name = "GetUsuarioById")]
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

        /// <summary>Cria um novo usuário.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(Usuario), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] UsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.EhVoluntario != 0 && dto.EhVoluntario != 1)
                return BadRequest("Campo EhVoluntario inválido. Use 1 para Sim ou 0 para Não.");

            // ⛔ RATE LIMITING
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconhecido";
            if (!Helpers.RateLimitStore.PodeExecutar($"post_usuario_{ip}", TimeSpan.FromSeconds(10)))
            {
                return StatusCode(429, new { mensagem = "Limite de requisições excedido. Tente novamente em alguns instantes." });
            }

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

        /// <summary>Atualiza um usuário existente.</summary>
        [HttpPut("{id}", Name = "AtualizarUsuario")]
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

        /// <summary>Deleta um usuário pelo ID.</summary>
        [HttpDelete("{id}", Name = "DeletarUsuario")]
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
