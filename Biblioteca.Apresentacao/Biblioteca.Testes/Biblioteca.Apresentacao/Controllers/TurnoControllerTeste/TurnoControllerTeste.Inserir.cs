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
        public async Task Inserir_DeveRetornarBadRequest()
        {
            var turnoParaInserir = new Turno
            {
                IdTurno = Guid.NewGuid(),
                Nome = "matutino",
            };

            var erroResponseMock = new ErrorResponse()
            {
                Mensagem = "Ocorreram alguns erros de validação",
                Status = 400,
                Erros = new Dictionary<string, List<string>> 
                { 
                    { "nome", new List<string>() { "nome do turno ja existe"} } 
                }
            };

            var turnoRepositorioMock = new TurnoRepositorioMock().Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarInserirTurno(erroResponseMock)
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Inserir(turnoParaInserir) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
            Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
        }

        [TestMethod]
        public async Task Inserir_DeveRetornarCreated()
        {
            var turnoParaInserir = new Turno()
            {
                IdTurno = Guid.NewGuid(),
                Nome = "integral"
            };

            var turnoServicoMock = new TurnoServicoMock().Build();

            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_Inserir()
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Inserir(turnoParaInserir) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(CreatedAtActionResult));

        }

        [TestMethod]
        public void Inserir_DeveRetornarInternalServerError()
        {
            var turnoParaInserir = new Turno()
            {
                IdTurno = Guid.NewGuid(),
                Nome = "integral"
            };

            var turnoServicoMock = new TurnoServicoMock().Build();

            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_InserirComExcessao()
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                var resultado = await turnoController.Inserir(turnoParaInserir) as ObjectResult;

                Assert.IsNotNull(resultado);
                Assert.AreEqual(500, resultado.StatusCode);
                Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
            });
        }

    }
}
