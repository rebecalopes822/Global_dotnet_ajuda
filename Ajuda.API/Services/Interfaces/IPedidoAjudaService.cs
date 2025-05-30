using Ajuda.API.DTOs;
using Ajuda.API.Models;

namespace Ajuda.API.Services.Interfaces
{
    public interface IPedidoAjudaService
    {
        Task<List<PedidoAjudaDetalhadoDto>> ListarTodosAsync();
        Task<PedidoAjudaDetalhadoDto?> ObterPorIdAsync(int id);
        Task<PedidoAjudaDetalhadoDto> CriarAsync(PedidoAjudaDto dto);
        Task<bool> AtualizarAsync(int id, PedidoAjudaDto dto);
        Task<bool> DeletarAsync(int id);

        Task<PedidoAjuda> MapearParaEntidade(PedidoAjudaDto dto);
        Task<PedidoAjuda> CriarDiretoAsync(PedidoAjuda entidade);
    }
}
