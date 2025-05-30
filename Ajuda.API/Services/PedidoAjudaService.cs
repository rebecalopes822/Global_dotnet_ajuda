using Ajuda.API.DTOs;
using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;

namespace Ajuda.API.Services
{
    public class PedidoAjudaService : IPedidoAjudaService
    {
        private readonly IPedidoAjudaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoAjudaRepository _tipoAjudaRepository;

        public PedidoAjudaService(
            IPedidoAjudaRepository repository,
            IUsuarioRepository usuarioRepository,
            ITipoAjudaRepository tipoAjudaRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _tipoAjudaRepository = tipoAjudaRepository;
        }

        public async Task<List<PedidoAjudaDetalhadoDto>> ListarTodosAsync()
        {
            var pedidos = await _repository.ListarTodosAsync();

            return pedidos.Select(p => new PedidoAjudaDetalhadoDto
            {
                Id = p.Id,
                UsuarioId = p.UsuarioId,
                NomeUsuario = p.Usuario?.Nome,
                TipoAjudaId = p.TipoAjudaId,
                NomeTipoAjuda = p.TipoAjuda?.Nome,
                Endereco = p.Endereco,
                QuantidadePessoas = p.QuantidadePessoas,
                NivelUrgencia = p.NivelUrgencia,
                DataHoraPedido = p.DataHoraPedido
            }).ToList();
        }

        public async Task<PedidoAjudaDetalhadoDto?> ObterPorIdAsync(int id)
        {
            var pedido = await _repository.ObterPorIdAsync(id);
            if (pedido == null)
                return null;

            return new PedidoAjudaDetalhadoDto
            {
                Id = pedido.Id,
                UsuarioId = pedido.UsuarioId,
                NomeUsuario = pedido.Usuario?.Nome,
                TipoAjudaId = pedido.TipoAjudaId,
                NomeTipoAjuda = pedido.TipoAjuda?.Nome,
                Endereco = pedido.Endereco,
                QuantidadePessoas = pedido.QuantidadePessoas,
                NivelUrgencia = pedido.NivelUrgencia,
                DataHoraPedido = pedido.DataHoraPedido
            };
        }

        public async Task<PedidoAjudaDetalhadoDto> CriarAsync(PedidoAjudaDto dto)
        {
            if (dto.NivelUrgencia > 5)
                throw new Exception("O nível de urgência deve ser no máximo 5.");

            var usuario = await _usuarioRepository.ObterPorIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            var tipoAjuda = await _tipoAjudaRepository.ObterPorIdAsync(dto.TipoAjudaId);
            if (tipoAjuda == null)
                throw new Exception("Tipo de ajuda não encontrado.");

            var pedido = new PedidoAjuda
            {
                UsuarioId = dto.UsuarioId,
                TipoAjudaId = dto.TipoAjudaId,
                Endereco = dto.Endereco,
                QuantidadePessoas = dto.QuantidadePessoas,
                NivelUrgencia = dto.NivelUrgencia,
                DataHoraPedido = DateTime.Now
            };

            var criado = await _repository.CriarAsync(pedido);

            return new PedidoAjudaDetalhadoDto
            {
                Id = criado.Id,
                UsuarioId = criado.UsuarioId,
                NomeUsuario = usuario.Nome,
                TipoAjudaId = criado.TipoAjudaId,
                NomeTipoAjuda = tipoAjuda.Nome,
                Endereco = criado.Endereco,
                QuantidadePessoas = criado.QuantidadePessoas,
                NivelUrgencia = criado.NivelUrgencia,
                DataHoraPedido = criado.DataHoraPedido
            };
        }

        public async Task<bool> AtualizarAsync(int id, PedidoAjudaDto dto)
        {
            var existente = await _repository.ObterPorIdAsync(id);
            if (existente == null)
                return false;

            existente.UsuarioId = dto.UsuarioId;
            existente.TipoAjudaId = dto.TipoAjudaId;
            existente.Endereco = dto.Endereco;
            existente.QuantidadePessoas = dto.QuantidadePessoas;
            existente.NivelUrgencia = dto.NivelUrgencia;

            await _repository.AtualizarAsync(existente);
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var existente = await _repository.ObterPorIdAsync(id);
            if (existente == null)
                return false;

            await _repository.DeletarAsync(id);
            return true;
        }

        public async Task<PedidoAjuda> MapearParaEntidade(PedidoAjudaDto dto)
        {
            return new PedidoAjuda
            {
                UsuarioId = dto.UsuarioId,
                TipoAjudaId = dto.TipoAjudaId,
                Endereco = dto.Endereco,
                QuantidadePessoas = dto.QuantidadePessoas,
                NivelUrgencia = dto.NivelUrgencia,
                DataHoraPedido = DateTime.Now
            };
        }

        public async Task<PedidoAjuda> CriarDiretoAsync(PedidoAjuda entidade)
        {
            await _repository.CriarAsync(entidade);
            return entidade;
        }
    }
}
