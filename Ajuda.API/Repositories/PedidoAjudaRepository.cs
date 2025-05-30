using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ajuda.API.Repositories
{
    public class PedidoAjudaRepository : IPedidoAjudaRepository
    {
        private readonly AppDbContext _context;

        public PedidoAjudaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PedidoAjuda>> ListarTodosAsync()
        {
            return await _context.PedidosAjuda
                .Include(p => p.Usuario)
                .Include(p => p.TipoAjuda)
                .ToListAsync();
        }

        public async Task<PedidoAjuda?> ObterPorIdAsync(int id)
        {
            return await _context.PedidosAjuda
                .Include(p => p.Usuario)
                .Include(p => p.TipoAjuda)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PedidoAjuda> CriarAsync(PedidoAjuda pedido)
        {
            _context.PedidosAjuda.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task AtualizarAsync(PedidoAjuda pedido)
        {
            _context.PedidosAjuda.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var pedido = await ObterPorIdAsync(id);
            if (pedido != null)
            {
                _context.PedidosAjuda.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
