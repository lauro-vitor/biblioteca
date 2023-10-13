using Biblioteca.Dominio.ViewModel;

namespace Biblioteca.Testes.Biblioteca.Dominio
{
    [TestClass]
    public class ErroResponseTeste
    {
        [TestMethod]
        public void DeveAtribuirErro()
        {
            var erroResponse = new ErroViewModel();

            erroResponse.AtribuirErro("nome", "nome ja existe");
            erroResponse.AtribuirErro("nome", "nome não pode conter espaços em branco");
            erroResponse.AtribuirErro("id", "id invalido");


            Assert.IsTrue(erroResponse.Erros.ContainsKey("nome"));
            Assert.IsTrue(erroResponse.Erros["nome"].Count == 2);
            Assert.IsTrue(erroResponse.Erros["nome"][0].Equals("nome ja existe"));
            Assert.IsTrue(erroResponse.Erros["nome"][1].Equals("nome não pode conter espaços em branco"));

            Assert.IsTrue(erroResponse.Erros.ContainsKey("id"));
            Assert.IsTrue(erroResponse.Erros["id"].Count == 1);
            Assert.IsTrue(erroResponse.Erros["id"][0].Equals("id invalido"));
        }
    }
}
