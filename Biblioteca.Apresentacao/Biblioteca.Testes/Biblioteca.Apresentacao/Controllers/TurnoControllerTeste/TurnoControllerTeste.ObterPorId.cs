using Biblioteca.Apresentacao.Controllers;
using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Testes.Mocks.Repositorio;
using Biblioteca.Testes.Mocks.Servico;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers.TurnoControllerTeste
{
    public partial class TurnoControllerTeste
    {
        [TestMethod]
        public async Task ObterPorId_DeveRetornarBadRequestIdObrigatorio()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_ObterPorId(null)
                .Build();

            var turnoServicoMock = new TurnoServicoMock().Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.ObterPorId(null) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
            Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
        }

        [TestMethod]
        public async Task ObterPorId_DeveRetornarNotFound()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_ObterPorId(null)
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.ObterPorId(Guid.Empty) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(NotFoundObjectResult));
            Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
        }

        [TestMethod]
        public async Task ObterPorId_DeveRetornarOk()
        {
            var turno = new Turno()
            {
                IdTurno = Guid.NewGuid(),
                Nome = "matutino"
            };

            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_ObterPorId(turno)
                .Build();

            var turnoServicoMock = new TurnoServicoMock().Build();

            var controller = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await controller.ObterPorId(Guid.Empty) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
            Assert.IsInstanceOfType(resultado.Value, typeof(Turno));
        }

        [TestMethod]
        public void ObterPorId_DeveRetornarInternalServerError()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_ObterPorIdExcessao()
                .Build();

            var turnoServicoMock = new TurnoServicoMock().Build();

            var controller = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                var resultado = await controller.ObterPorId(Guid.Empty) as ObjectResult;

                Assert.IsNotNull(resultado);
                Assert.AreEqual(500, resultado.StatusCode);
                Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
            });
        }
    }
}
