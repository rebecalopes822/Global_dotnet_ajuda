using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;

namespace Ajuda.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Usuario>> ListarAsync() => await _repository.ListarAsync();
        public async Task<Usuario?> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);
        public async Task<Usuario> CriarAsync(Usuario usuario) => await _repository.CriarAsync(usuario);
        public async Task AtualizarAsync(Usuario usuario) => await _repository.AtualizarAsync(usuario);
        public async Task DeletarAsync(int id) => await _repository.DeletarAsync(id);
    }
}
