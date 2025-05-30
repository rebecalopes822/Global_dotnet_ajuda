using Ajuda.API.Models;

namespace Ajuda.API.Services.Interfaces
{
    public interface ITipoAjudaRepository
    {
        Task<List<TipoAjuda>> ListarAsync();
        Task<TipoAjuda> ObterPorIdAsync(int id);
        Task<TipoAjuda> CriarAsync(TipoAjuda tipo);
        Task AtualizarAsync(TipoAjuda tipo);
        Task DeletarAsync(int id);
    }
}
