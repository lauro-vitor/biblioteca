using Biblioteca.Apresentacao.Controllers;
using Biblioteca.Dominio.Entidades;
using Biblioteca.Testes.Mocks.Repositorio;
using Biblioteca.Testes.Mocks.Servico;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers.TurnoControllerTeste
{

    [TestClass]
    public partial class TurnoControllerTeste
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
        public void TestarViews()
        {
            var turnoRepositorioMock = new TurnoRepositorioMock().Build();

            var turnoServicoMock = new TurnoServicoMock().Build();

            var turnoController = new TurnoController(turnoRepositorioMock, turnoServicoMock);

            ViewResult indexViewResult = turnoController.IndexVW();
            ViewResult inserirViewResult = turnoController.InserirVW();
            ViewResult editarViewResult = turnoController.EditarVW(Guid.Empty);
            ViewResult excluirViewResult = turnoController.ExcluirVW(Guid.Empty);

            Action<ViewResult, string> testarView = (v, name) =>
            {
                Assert.IsNotNull(v);
                Assert.AreEqual(v.ViewName, name);
            };

            testarView(indexViewResult, "~/Views/Turno/Index.cshtml");
            testarView(inserirViewResult, "~/Views/Turno/Inserir.cshtml");
            testarView(editarViewResult, "~/Views/Turno/Editar.cshtml");
            testarView(excluirViewResult, "~/Views/Turno/Excluir.cshtml");
        }

    }
}
