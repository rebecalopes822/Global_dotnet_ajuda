using Ajuda.API.Models;

namespace Ajuda.API.Services.Interfaces
{
    public interface ITipoAjudaService
    {
        Task<List<TipoAjuda>> ListarAsync();
        Task<TipoAjuda?> ObterPorIdAsync(int id);
        Task<TipoAjuda> CriarAsync(TipoAjuda tipoAjuda);
        Task AtualizarAsync(TipoAjuda tipoAjuda);
        Task DeletarAsync(int id);
    }
}
