using Biblioteca.Apresentacao.Controllers;
using Biblioteca.Dominio.DTO;
using Biblioteca.Testes.Mocks.Repositorio;
using Biblioteca.Testes.Mocks.Servico;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers.TurnoControllerTeste
{
    public partial class TurnoControllerTeste
    {

        [TestMethod]
        public async Task Obter_DeveRetornraOk()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_Obter(_turnosMock)
                .Build();

            var turnoServicoMock = new TurnoServicoMock().Build();

            var controller = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await controller.Obter();

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
        }

        [TestMethod]

        public void Obter_DeveRetornraErro500()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_ObterComExcessao()
                .Build();

            var turnoServicoMock = new TurnoServicoMock().Build();

            var controller = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                var resultado = await controller.Obter() as ObjectResult;

                Assert.IsNotNull(resultado);
                Assert.AreEqual(resultado.StatusCode, 500);
                Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
            });
        }

    }
}
