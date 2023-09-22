using Biblioteca.Dominio.Entidades;
using Biblioteca.Dominio.Servico;
using Biblioteca.Servico;
using System.Security.Cryptography.X509Certificates;

namespace Biblioteca.Testes.Biblioteca.Servico
{
    [TestClass]
    public class TurnoServicoTestes
    {
        private readonly IList<Turno> _turnoCadastros = new List<Turno>()
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
        public void ValidarInserirTurno_DeveRetornarNomeObrigatorio()
        {
            var turnoServico = new TurnoServico();
            var turnoParaInserir = new Turno();
            var turnosCadastrados = new List<Turno>();

            var erroResponse = turnoServico.ValidarInserirTurno(turnoParaInserir, turnosCadastrados);

            Assert.IsNotNull(erroResponse);
            Assert.IsTrue(erroResponse.Erros.ContainsKey("nome"));
            Assert.AreEqual(400, erroResponse.Status);
            Assert.AreEqual("Ocorreram alguns erros de validação", erroResponse.Mensagem);
            Assert.AreEqual("nome do turno é obrigatório", erroResponse.Erros["nome"][0]);
        }

        [TestMethod]
        public void ValidarInserirTurno_DeveRetornarNomeExiste()
        {
            var turnoServico = new TurnoServico();

            var turnoParaInserir = new Turno()
            {
                Nome = " matutino "
            };

            var erroResponse = turnoServico.ValidarInserirTurno(turnoParaInserir, _turnoCadastros);

            Assert.IsNotNull(erroResponse);
            Assert.IsTrue(erroResponse.Erros.ContainsKey("nome"));
            Assert.AreEqual(400, erroResponse.Status);
            Assert.AreEqual("Ocorreram alguns erros de validação", erroResponse.Mensagem);
            Assert.AreEqual("nome do turno existe", erroResponse.Erros["nome"][0]);
        }

        [TestMethod]
        public void ValidarInserirTurno_DeveRetornarNulo()
        {
            var turnoServico = new TurnoServico();

            var turnoParaInserir = new Turno()
            {
                Nome = "integral"
            };

            var erroResponse = turnoServico.ValidarInserirTurno(turnoParaInserir, _turnoCadastros);

            Assert.IsNull(erroResponse);
        }



        [TestMethod]
        public void ValidarEditarTurno_DeveRetornarIdObrigatorioGuidEmpty()
        {
            var turnoServico = new TurnoServico();

            var turnoParaEditar = new Turno()
            {
                Nome = "teste"
            };

            var erroResponse = turnoServico.ValidarEditarTurno(turnoParaEditar, _turnoCadastros);

            Assert.IsNotNull(erroResponse);
            Assert.IsTrue(erroResponse.Erros.ContainsKey("idturno"));
            Assert.AreEqual(400, erroResponse.Status);
            Assert.AreEqual("id do turno é obrigatório", erroResponse.Erros["idturno"][0]);
        }

        [TestMethod]
        public void ValidarEditarTurno_DeveRetornarNomeObrigatorio()
        {
            var turnoServico = new TurnoServico();

            var turnoParaEditar = new Turno()
            {
                Nome = ""
            };

            var erroResponse = turnoServico.ValidarEditarTurno(turnoParaEditar, _turnoCadastros);

            Assert.IsNotNull(erroResponse);
            Assert.IsTrue(erroResponse.Erros.ContainsKey("nome"));
            Assert.AreEqual(400, erroResponse.Status);
            Assert.AreEqual("nome do turno é obrigatório",erroResponse.Erros["nome"][0]);
        }

        [TestMethod]
        public void ValidarEditarTurno_DeveRetornarNomeExistente()
        {
            var turnoServico = new TurnoServico();

            var turnoParaEditar = _turnoCadastros.First(t => t.Nome.Equals("Matutino"));

            turnoParaEditar.Nome = "vespertino";

            var erroResponse = turnoServico.ValidarEditarTurno(turnoParaEditar, _turnoCadastros);

            Assert.IsNotNull(erroResponse);
            Assert.IsTrue(erroResponse.Erros.ContainsKey("nome"));
            Assert.AreEqual(400, erroResponse.Status);
            Assert.AreEqual("nome do turno existe", erroResponse.Erros["nome"][0]);
        }

        [TestMethod]
        public void ValidarEditarTurno_DeveRetornarNulo()
        {
            var turnoServico = new TurnoServico();

            var turnoParaEditar = _turnoCadastros.First(t => t.Nome.Equals("Matutino"));

            turnoParaEditar.Nome = "matutino";

            var erroResponse = turnoServico.ValidarEditarTurno(turnoParaEditar, _turnoCadastros);

            Assert.IsNull(erroResponse);
        }

        [TestMethod]
        public void ValidarTurnoNaoExiste_DeveRetornarErro()
        {
            var turnoServico = new TurnoServico();
            var id = Guid.NewGuid();

            var erroResponse = turnoServico.ValidarTurnoNaoExiste(id, _turnoCadastros);

            Assert.IsNotNull(erroResponse);
            Assert.IsTrue(erroResponse.Erros.ContainsKey("idturno"));
            Assert.AreEqual(erroResponse.Status, 404);
            Assert.AreEqual(erroResponse.Erros["idturno"][0], "Turno não encontrado para esse ID");
        }


        [TestMethod]
        public void ValidarTurnoNaoExiste_DeveRetornarNulo()
        {
            var turnoServico = new TurnoServico();
            var id = new Guid("f0c65772-615e-4b6e-9346-8cf463e8a8c1");

            var erroResponse = turnoServico.ValidarTurnoNaoExiste(id, _turnoCadastros);

            Assert.IsNull(erroResponse);
        }
    }
}
