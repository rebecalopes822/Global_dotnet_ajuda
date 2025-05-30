using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Ajuda.API.Services;
using Moq;

namespace Ajuda.API.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepo;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            _mockRepo = new Mock<IUsuarioRepository>();
            _service = new UsuarioService(_mockRepo.Object);
        }

        [Fact]
        public async Task CriarAsync_DeveRetornarUsuarioCriado()
        {
            var usuario = new Usuario { Id = 1, Nome = "Teste" };

            _mockRepo.Setup(r => r.CriarAsync(usuario)).ReturnsAsync(usuario);

            var resultado = await _service.CriarAsync(usuario);

            Assert.Equal(usuario.Id, resultado.Id);
            Assert.Equal("Teste", resultado.Nome);
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarLista()
        {
            var lista = new List<Usuario>
            {
                new Usuario { Id = 1 },
                new Usuario { Id = 2 }
            };

            _mockRepo.Setup(r => r.ListarAsync()).ReturnsAsync(lista);

            var resultado = await _service.ListarAsync();

            Assert.Equal(2, resultado.Count);
        }
    }
}
