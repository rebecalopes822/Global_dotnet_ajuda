using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;

namespace Ajuda.API.Services
{
    public class TipoAjudaService : ITipoAjudaService
    {
        private readonly ITipoAjudaRepository _repository;

        public TipoAjudaService(ITipoAjudaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TipoAjuda>> ListarAsync()
        {
            return await _repository.ListarAsync();
        }

        public async Task<TipoAjuda?> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task<TipoAjuda> CriarAsync(TipoAjuda tipo)
        {
            return await _repository.CriarAsync(tipo);
        }

        public async Task AtualizarAsync(TipoAjuda tipo)
        {
            await _repository.AtualizarAsync(tipo);
        }

        public async Task DeletarAsync(int id)
        {
            await _repository.DeletarAsync(id);
        }
    }
}
