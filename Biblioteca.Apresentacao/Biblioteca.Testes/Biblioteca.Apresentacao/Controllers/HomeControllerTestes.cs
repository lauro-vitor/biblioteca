using Biblioteca.Apresentacao.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Testes.Biblioteca.Apresentacao.Controllers
{
    [TestClass]
    public class HomeControllerTestes
    {
        [TestMethod]
        public void Index_VerificarTitlePagina()
        {
            var homeController = new HomeController();

            var view = homeController.Index();

            Assert.AreEqual("Home page", view.ViewData["title"]);
        }
    }
}
