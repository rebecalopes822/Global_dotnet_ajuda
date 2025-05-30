using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ajuda.API.Repositories
{
    public class TipoAjudaRepository : ITipoAjudaRepository
    {
        private readonly AppDbContext _context;

        public TipoAjudaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoAjuda>> ListarAsync()
        {
            return await _context.TiposAjuda.ToListAsync();
        }

        public async Task<TipoAjuda?> ObterPorIdAsync(int id)
        {
            return await _context.TiposAjuda.FindAsync(id);
        }

        public async Task<TipoAjuda> CriarAsync(TipoAjuda tipoAjuda)
        {
            _context.TiposAjuda.Add(tipoAjuda);
            await _context.SaveChangesAsync();
            return tipoAjuda;
        }

        public async Task AtualizarAsync(TipoAjuda tipoAjuda)
        {
            _context.TiposAjuda.Update(tipoAjuda);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(int id)
        {
            var tipoAjuda = await ObterPorIdAsync(id);
            if (tipoAjuda != null)
            {
                _context.TiposAjuda.Remove(tipoAjuda);
                await _context.SaveChangesAsync();
            }
        }
    }
}
