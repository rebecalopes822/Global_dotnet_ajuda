using Ajuda.API.Controllers;
using Ajuda.API.DTOs;
using Ajuda.API.Mensageria;
using Ajuda.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ajuda.API.Tests.Controllers
{
    public class PedidoAjudaControllerTests
    {
        private readonly Mock<IPedidoAjudaService> _serviceMock;
        private readonly Mock<PedidoAjudaQueue> _queueMock;
        private readonly PedidoAjudaController _controller;

        public PedidoAjudaControllerTests()
        {
            _serviceMock = new Mock<IPedidoAjudaService>();
            _queueMock = new Mock<PedidoAjudaQueue>();
            _controller = new PedidoAjudaController(_serviceMock.Object, _queueMock.Object);
        }

        [Fact]
        public async Task Get_DeveRetornarOkComListaDePedidos()
        {
            // Arrange
            var pedidos = new List<PedidoAjudaDetalhadoDto>
            {
                new PedidoAjudaDetalhadoDto { Id = 1, Endereco = "Rua A" },
                new PedidoAjudaDetalhadoDto { Id = 2, Endereco = "Rua B" }
            };
            _serviceMock.Setup(s => s.ListarTodosAsync()).ReturnsAsync(pedidos);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var listaRetornada = Assert.IsAssignableFrom<IEnumerable<PedidoAjudaDetalhadoDto>>(okResult.Value);
            Assert.Equal(2, ((List<PedidoAjudaDetalhadoDto>)listaRetornada).Count);
        }

        [Fact]
        public async Task GetPorId_DeveRetornarNotFoundSeNaoExistir()
        {
            // Arrange
            _serviceMock.Setup(s => s.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((PedidoAjudaDetalhadoDto?)null);

            // Act
            var resultado = await _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(resultado);
        }

        [Fact]
        public async Task Post_DeveRetornarBadRequestSeModelStateInvalido()
        {
            // Arrange
            _controller.ModelState.AddModelError("Endereco", "Endereço é obrigatório");

            // Act
            var resultado = await _controller.Post(new PedidoAjudaDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact]
        public async Task Put_DeveRetornarNoContentQuandoAtualizarComSucesso()
        {
            // Arrange
            var dto = new PedidoAjudaDto
            {
                UsuarioId = 1,
                TipoAjudaId = 1,
                Endereco = "Rua Atualizada",
                QuantidadePessoas = 4,
                NivelUrgencia = 3
            };

            _serviceMock.Setup(s => s.AtualizarAsync(1, dto)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Put(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public async Task Put_DeveRetornarNotFoundSeNaoExistirPedido()
        {
            // Arrange
            _serviceMock.Setup(s => s.AtualizarAsync(999, It.IsAny<PedidoAjudaDto>())).ReturnsAsync(false);

            // Act
            var resultado = await _controller.Put(999, new PedidoAjudaDto());

            // Assert
            Assert.IsType<NotFoundObjectResult>(resultado);
        }

        [Fact]
        public async Task Delete_DeveRetornarNoContentQuandoDeletarComSucesso()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeletarAsync(1)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFoundSeNaoExistirPedido()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeletarAsync(999)).ReturnsAsync(false);

            // Act
            var resultado = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(resultado);
        }
    }
}
