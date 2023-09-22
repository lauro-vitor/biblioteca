using Biblioteca.Apresentacao.Controllers;
using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Testes.Mocks.Repositorio;
using Biblioteca.Testes.Mocks.Servico;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers
{
    [TestClass]
    public class TurnoControllerTestes
    {
        private readonly IList<Turno> _turnosMock = new List<Turno>()
        {
            new Turno()
            {
                IdTurno = Guid.Parse("f0c65772-615e-4b6e-9346-8cf463e8a8c1"),
                Nome = "Matutino",
            },
            new Turno()
            {
                IdTurno = Guid.Parse("5cbe2213-0f32-46be-b440-d222bf72eae5"),
                Nome = "Vespertino",
            },
            new Turno()
            {
                IdTurno = Guid.Parse("ead74605-5417-4ca2-9bfa-4037346f6860"),
                Nome = "Noturno",
            }
        };

        [TestMethod]
        public async Task Obter_DeveRetornraOk()
        {
            var turnoRepositorioMock = TurnoRepositorioMock
                .Instance()
                .Configurar_Obter(_turnosMock)
                .Build();

            var turnoServicoMock = TurnoServicoMock
                .Instance()
                .Build();
            
            var controller = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await controller.Obter();

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));
        }

        [TestMethod]

        public void Obter_DeveRetornraErro500()
        {
            var turnoRepositorioMock = TurnoRepositorioMock
                .Instance()
                .Configurar_ObterComExcessao()
                .Build();

            var turnoServicoMock = TurnoServicoMock.Instance().Build();

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
