using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Ajuda.API.Services;
using Moq;

namespace Ajuda.API.Tests.Services
{
    public class TipoAjudaServiceTests
    {
        private readonly Mock<ITipoAjudaRepository> _mockRepo;
        private readonly TipoAjudaService _service;

        public TipoAjudaServiceTests()
        {
            _mockRepo = new Mock<ITipoAjudaRepository>();
            _service = new TipoAjudaService(_mockRepo.Object);
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarListaDeTipos()
        {
            var lista = new List<TipoAjuda>
            {
                new TipoAjuda { Id = 1, Nome = "Água", Descricao = "Ajuda com água potável" },
                new TipoAjuda { Id = 2, Nome = "Comida", Descricao = "Ajuda com alimentos" }
            };
            _mockRepo.Setup(r => r.ListarAsync()).ReturnsAsync(lista);

            var resultado = await _service.ListarAsync();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarTipoAjuda_QuandoEncontrado()
        {
            var tipo = new TipoAjuda { Id = 1, Nome = "Água", Descricao = "Ajuda com água potável" };
            _mockRepo.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(tipo);

            var resultado = await _service.ObterPorIdAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal("Água", resultado!.Nome);
        }

        [Fact]
        public async Task CriarAsync_DeveRetornarTipoAjudaCriado()
        {
            var tipo = new TipoAjuda { Id = 3, Nome = "Abrigo", Descricao = "Ajuda com abrigo" };
            _mockRepo.Setup(r => r.CriarAsync(tipo)).ReturnsAsync(tipo);

            var resultado = await _service.CriarAsync(tipo);

            Assert.NotNull(resultado);
            Assert.Equal("Abrigo", resultado.Nome);
        }

        [Fact]
        public async Task AtualizarAsync_DeveChamarMetodoDoRepositorio()
        {
            var tipo = new TipoAjuda { Id = 1, Nome = "Água", Descricao = "Atualizado" };
            _mockRepo.Setup(r => r.AtualizarAsync(tipo)).Returns(Task.CompletedTask);

            await _service.AtualizarAsync(tipo);

            _mockRepo.Verify(r => r.AtualizarAsync(tipo), Times.Once);
        }

        [Fact]
        public async Task DeletarAsync_DeveChamarMetodoDoRepositorio()
        {
            int id = 1;
            _mockRepo.Setup(r => r.DeletarAsync(id)).Returns(Task.CompletedTask);

            await _service.DeletarAsync(id);

            _mockRepo.Verify(r => r.DeletarAsync(id), Times.Once);
        }
    }
}
