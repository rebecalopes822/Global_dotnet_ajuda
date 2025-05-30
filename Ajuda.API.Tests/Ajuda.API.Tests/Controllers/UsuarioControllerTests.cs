using Ajuda.API.Controllers;
using Ajuda.API.DTOs;
using Ajuda.API.Models;
using Ajuda.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ajuda.API.Tests.Controllers
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUsuarioService> _mockService;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _mockService = new Mock<IUsuarioService>();
            _controller = new UsuarioController(_mockService.Object);
        }

        [Fact]
        public async Task Get_DeveRetornarUsuariosComOk()
        {
            var lista = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "Teste", Email = "teste@teste.com", Telefone = "123", EhVoluntario = 1 }
            };
            _mockService.Setup(s => s.ListarAsync()).ReturnsAsync(lista);

            var resultado = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.IsAssignableFrom<IEnumerable<UsuarioComLinksDto>>(okResult.Value);
        }

        [Fact]
        public async Task Get_ById_DeveRetornarNotFoundSeNaoEncontrar()
        {
            _mockService.Setup(s => s.ObterPorIdAsync(999)).ReturnsAsync((Usuario?)null);

            var resultado = await _controller.Get(999);

            Assert.IsType<NotFoundObjectResult>(resultado);
        }
    }
}
