using Ajuda.API.Models;
using Ajuda.API.Repositories;
using Ajuda.API.Services.Interfaces;

namespace Ajuda.API.Services
{
    public class TipoAjudaService : ITipoAjudaService
    {
        private readonly TipoAjudaRepository _repository;

        public TipoAjudaService(TipoAjudaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TipoAjuda>> ListarAsync() => await _repository.ListarAsync();

        public async Task<TipoAjuda?> ObterPorIdAsync(int id) => await _repository.ObterPorIdAsync(id);

        public async Task<TipoAjuda> CriarAsync(TipoAjuda tipoAjuda) => await _repository.CriarAsync(tipoAjuda);

        public async Task AtualizarAsync(TipoAjuda tipoAjuda) => await _repository.AtualizarAsync(tipoAjuda);

        public async Task DeletarAsync(int id) => await _repository.DeletarAsync(id);
    }
}
