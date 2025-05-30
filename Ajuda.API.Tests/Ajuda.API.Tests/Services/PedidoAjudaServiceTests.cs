using Ajuda.API.Models;
using Ajuda.API.Services;
using Ajuda.API.Services.Interfaces;
using Moq;


namespace Ajuda.API.Tests.Services
{
    public class PedidoAjudaServiceTests
    {
        private readonly Mock<IPedidoAjudaRepository> _pedidoRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<ITipoAjudaRepository> _tipoAjudaRepositoryMock;
        private readonly PedidoAjudaService _service;

        public PedidoAjudaServiceTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoAjudaRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _tipoAjudaRepositoryMock = new Mock<ITipoAjudaRepository>();

            _service = new PedidoAjudaService(
                _pedidoRepositoryMock.Object,
                _usuarioRepositoryMock.Object,
                _tipoAjudaRepositoryMock.Object);
        }

        [Fact]
        public async Task ListarTodosAsync_DeveRetornarListaDePedidos()
        {
            // Arrange
            var pedidos = new List<PedidoAjuda>
            {
                new PedidoAjuda { Id = 1, UsuarioId = 10, TipoAjudaId = 100, Endereco = "Rua A", QuantidadePessoas = 5, NivelUrgencia = 3, DataHoraPedido = DateTime.Now, Usuario = new Usuario { Nome = "Usu1" }, TipoAjuda = new TipoAjuda { Nome = "Água" } },
                new PedidoAjuda { Id = 2, UsuarioId = 20, TipoAjudaId = 200, Endereco = "Rua B", QuantidadePessoas = 2, NivelUrgencia = 1, DataHoraPedido = DateTime.Now, Usuario = new Usuario { Nome = "Usu2" }, TipoAjuda = new TipoAjuda { Nome = "Comida" } }
            };

            _pedidoRepositoryMock.Setup(r => r.ListarTodosAsync()).ReturnsAsync(pedidos);

            // Act
            var resultado = await _service.ListarTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, p => p.Endereco == "Rua A" && p.NomeUsuario == "Usu1");
            Assert.Contains(resultado, p => p.Endereco == "Rua B" && p.NomeTipoAjuda == "Comida");
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarPedidoQuandoEncontrado()
        {
            // Arrange
            var pedido = new PedidoAjuda
            {
                Id = 1,
                UsuarioId = 10,
                TipoAjudaId = 100,
                Endereco = "Rua A",
                QuantidadePessoas = 5,
                NivelUrgencia = 3,
                DataHoraPedido = DateTime.Now,
                Usuario = new Usuario { Nome = "Usu1" },
                TipoAjuda = new TipoAjuda { Nome = "Água" }
            };

            _pedidoRepositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(pedido);

            // Act
            var resultado = await _service.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Usu1", resultado.NomeUsuario);
            Assert.Equal("Água", resultado.NomeTipoAjuda);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNullQuandoNaoEncontrado()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((PedidoAjuda?)null);

            // Act
            var resultado = await _service.ObterPorIdAsync(999);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task CriarAsync_DeveCriarPedidoQuandoDadosValidos()
        {
            // Arrange
            var dto = new PedidoAjudaDto
            {
                UsuarioId = 10,
                TipoAjudaId = 100,
                Endereco = "Rua Teste",
                QuantidadePessoas = 4,
                NivelUrgencia = 3
            };

            var usuario = new Usuario { Id = 10, Nome = "Usuário Teste" };
            var tipoAjuda = new TipoAjuda { Id = 100, Nome = "Água" };

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(dto.UsuarioId)).ReturnsAsync(usuario);
            _tipoAjudaRepositoryMock.Setup(r => r.ObterPorIdAsync(dto.TipoAjudaId)).ReturnsAsync(tipoAjuda);

            _pedidoRepositoryMock.Setup(r => r.CriarAsync(It.IsAny<PedidoAjuda>()))
                .ReturnsAsync((PedidoAjuda p) =>
                {
                    p.Id = 1; // Simula ID atribuído pelo BD
                    return p;
                });

            // Act
            var resultado = await _service.CriarAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal(dto.Endereco, resultado.Endereco);
            Assert.Equal(usuario.Nome, resultado.NomeUsuario);
            Assert.Equal(tipoAjuda.Nome, resultado.NomeTipoAjuda);
        }

        [Fact]
        public async Task CriarAsync_DeveLancarExcecaoSeNivelUrgenciaMaiorQue5()
        {
            // Arrange
            var dto = new PedidoAjudaDto
            {
                UsuarioId = 10,
                TipoAjudaId = 100,
                Endereco = "Rua Teste",
                QuantidadePessoas = 4,
                NivelUrgencia = 6
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CriarAsync(dto));
            Assert.Equal("O nível de urgência deve ser no máximo 5.", ex.Message);
        }

        [Fact]
        public async Task CriarAsync_DeveLancarExcecaoSeUsuarioNaoEncontrado()
        {
            // Arrange
            var dto = new PedidoAjudaDto
            {
                UsuarioId = 999,
                TipoAjudaId = 100,
                Endereco = "Rua Teste",
                QuantidadePessoas = 4,
                NivelUrgencia = 3
            };

            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(dto.UsuarioId)).ReturnsAsync((Usuario?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CriarAsync(dto));
            Assert.Equal("Usuário não encontrado.", ex.Message);
        }

        [Fact]
        public async Task CriarAsync_DeveLancarExcecaoSeTipoAjudaNaoEncontrado()
        {
            // Arrange
            var dto = new PedidoAjudaDto
            {
                UsuarioId = 10,
                TipoAjudaId = 999,
                Endereco = "Rua Teste",
                QuantidadePessoas = 4,
                NivelUrgencia = 3
            };

            var usuario = new Usuario { Id = 10, Nome = "Usuário Teste" };
            _usuarioRepositoryMock.Setup(r => r.ObterPorIdAsync(dto.UsuarioId)).ReturnsAsync(usuario);
            _tipoAjudaRepositoryMock.Setup(r => r.ObterPorIdAsync(dto.TipoAjudaId)).ReturnsAsync((TipoAjuda?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CriarAsync(dto));
            Assert.Equal("Tipo de ajuda não encontrado.", ex.Message);
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarTrueQuandoPedidoExistir()
        {
            // Arrange
            var pedidoExistente = new PedidoAjuda
            {
                Id = 1,
                UsuarioId = 10,
                TipoAjudaId = 100,
                Endereco = "Rua Original",
                QuantidadePessoas = 2,
                NivelUrgencia = 2
            };

            var dto = new PedidoAjudaDto
            {
                UsuarioId = 20,
                TipoAjudaId = 200,
                Endereco = "Rua Atualizada",
                QuantidadePessoas = 5,
                NivelUrgencia = 4
            };

            _pedidoRepositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(pedidoExistente);
            _pedidoRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<PedidoAjuda>())).Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.AtualizarAsync(1, dto);

            // Assert
            Assert.True(resultado);
            _pedidoRepositoryMock.Verify(r => r.AtualizarAsync(It.Is<PedidoAjuda>(p =>
                p.Endereco == dto.Endereco &&
                p.QuantidadePessoas == dto.QuantidadePessoas &&
                p.NivelUrgencia == dto.NivelUrgencia &&
                p.UsuarioId == dto.UsuarioId &&
                p.TipoAjudaId == dto.TipoAjudaId)), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarFalseQuandoPedidoNaoExistir()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((PedidoAjuda?)null);

            // Act
            var resultado = await _service.AtualizarAsync(999, new PedidoAjudaDto());

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task DeletarAsync_DeveRetornarTrueQuandoPedidoExistir()
        {
            // Arrange
            var pedidoExistente = new PedidoAjuda { Id = 1 };
            _pedidoRepositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(pedidoExistente);
            _pedidoRepositoryMock.Setup(r => r.DeletarAsync(1)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.DeletarAsync(1);

            // Assert
            Assert.True(resultado);
            _pedidoRepositoryMock.Verify(r => r.DeletarAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeletarAsync_DeveRetornarFalseQuandoPedidoNaoExistir()
        {
            // Arrange
            _pedidoRepositoryMock.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((PedidoAjuda?)null);

            // Act
            var resultado = await _service.DeletarAsync(999);

            // Assert
            Assert.False(resultado);
        }
    }
}
