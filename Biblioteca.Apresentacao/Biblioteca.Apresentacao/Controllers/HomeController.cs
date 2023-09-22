using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Apresentacao.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
           
        }

        public ViewResult Index()
        {
            var view = View();

            view.ViewData["title"] = "Home page";

            return view;
        }
    }
}