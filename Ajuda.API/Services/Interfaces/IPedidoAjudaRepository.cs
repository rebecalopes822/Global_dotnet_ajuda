using Ajuda.API.Models;

namespace Ajuda.API.Services.Interfaces
{
    public interface IPedidoAjudaRepository
    {
        Task<List<PedidoAjuda>> ListarTodosAsync();
        Task<PedidoAjuda?> ObterPorIdAsync(int id);
        Task<PedidoAjuda> CriarAsync(PedidoAjuda pedido);
        Task AtualizarAsync(PedidoAjuda pedido);
        Task DeletarAsync(int id);
    }
}
