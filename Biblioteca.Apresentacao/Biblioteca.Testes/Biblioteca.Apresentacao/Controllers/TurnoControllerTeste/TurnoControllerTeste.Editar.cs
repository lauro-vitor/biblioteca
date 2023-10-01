using Biblioteca.Apresentacao.Controllers;
using Biblioteca.Dominio.ViewModel;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Testes.Mocks.Repositorio;
using Biblioteca.Testes.Mocks.Servico;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers.TurnoControllerTeste
{
    public partial class TurnoControllerTeste
    {
        private readonly Turno _turnoParaEditar = new Turno()
        {
            IdTurno = Guid.NewGuid(),
            Nome = "Turno teste"
        };

        private readonly ErroViewModel _erroResponseValidacao = new ErroViewModel()
        {
            Status = 400,
            Mensagem = "Erros de validação",
            Erros = new Dictionary<string, List<string>>
                {
                    {
                        "nome",
                        new List<string>
                        {
                            "Nome do turno inválido"
                        }
                    }
                }
        };

        [TestMethod]
        public async Task Editar_DeveRetornarBadRequestSeIdNulo()
        {
            var turnoRespositorioMock = new TurnoRepositorioMock()
                .Configurar_Editar()
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Build();

            var turnoController = new TurnoController(turnoRespositorioMock, turnoServicoMock);

            var resultado = await turnoController.Editar(null, new Turno()) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
            Assert.IsInstanceOfType(resultado.Value, typeof(ErroViewModel));
        }

        [TestMethod]
        public async Task Editar_DeveRetornarNotFound()
        {
        
            var turnoRespositorioMock = new TurnoRepositorioMock()
                .Configurar_Editar()
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarTurnoNaoExiste(_erroResponseValidacao)
                .Build();

            var turnoController = new TurnoController(turnoRespositorioMock, turnoServicoMock);

            var resultado = await turnoController.Editar(Guid.Empty, _turnoParaEditar) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(NotFoundObjectResult));
            Assert.IsInstanceOfType(resultado.Value, typeof(ErroViewModel));
        }

        [TestMethod]
        public async Task Editar_DeveRetornarOk()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_Editar()
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Editar(Guid.Empty, _turnoParaEditar) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));

        }

        [TestMethod]
        public async Task Editar_DeveRetornarBadRequestSeTurnoForInvalido()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_Editar()
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarEditarTurno(_erroResponseValidacao)
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            var resultado = await turnoController.Editar(Guid.NewGuid(), _turnoParaEditar) as ObjectResult;

            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Editar_DeveRetornarInternalServerError()
        {
            

            var turnoRepositorioMock = new TurnoRepositorioMock()
                .Configurar_EditarComExcessao()
                .Build();

            var turnoServicoMock = new TurnoServicoMock()
                .Configurar_ValidarTurnoNaoExiste(null)
                .Configurar_ValidarEditarTurno(null)
                .Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                var resultado = await turnoController.Editar(Guid.NewGuid(), _turnoParaEditar) as ObjectResult;

                Assert.IsNotNull(resultado);
                Assert.AreEqual(500, resultado.StatusCode);
                Assert.IsInstanceOfType(resultado.Value, typeof(ErroViewModel));

            });
            
        }

    }
}
