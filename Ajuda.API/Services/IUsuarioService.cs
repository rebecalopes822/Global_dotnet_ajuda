using Ajuda.API.Models;

namespace Ajuda.API.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ListarAsync();
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario> CriarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task DeletarAsync(int id);
    }
}
