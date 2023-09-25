using Biblioteca.Apresentacao.Controllers;
using Biblioteca.Dominio.DTO;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Testes.Mocks.Repositorio;
using Biblioteca.Testes.Mocks.Servico;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers.TurnoControllerTeste
{
    public partial class TurnoControllerTeste
    {
        
        [TestMethod]
        public async Task Excluir_DeveRetornarBadRequestSeIdNulo()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                 .Configurar_Excluir()
                 .Build();

            var turnoServicoMock = new TurnoServicoMock().Build();
            
            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Excluir(null) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));   
        }

        [TestMethod]
        public async Task Excluir_DeveRetornarBadRequestSeTurnoExistir()
        {
            var erroResponse = new ErrorResponse()
            {
                Mensagem = "Erro de validação",
                Erros = new Dictionary<string, List<string>>
                {
                    {
                        "idTurno",
                        new List<string>
                        {
                            "Não existe turno para excluir"
                        }
                    }
                }
            };

            var turnoRepositorioMock = new TurnoRepositorioMock()
                 .Configurar_Obter(It.IsAny<IList<Turno>>())
                 .Configurar_Excluir()
                 .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarTurnoNaoExiste(erroResponse)
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Excluir(Guid.NewGuid()) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Excluir_DeveRetornarNoContent()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_Obter(It.IsAny<IList<Turno>>())
                .Configurar_Excluir()
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarTurnoNaoExiste(null)
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Excluir(Guid.NewGuid());

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(NoContentResult));
        }

        [TestMethod]
        public void Excluir_DeveRetornarInternalServer()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
               .Configurar_Obter(It.IsAny<IList<Turno>>())
               .Configurar_ExcluirComExcessao()
               .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarTurnoNaoExiste(null)
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                var resultado = await turnoController.Excluir(Guid.NewGuid()) as ObjectResult;

                Assert.IsNotNull(resultado);
                Assert.AreEqual(500, resultado.StatusCode);
                Assert.IsInstanceOfType(resultado.Value, typeof(ErrorResponse));
            });
        }
    }
}
