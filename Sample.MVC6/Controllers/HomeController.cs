using System.Web.Mvc;
using Lion.Localization;

namespace Sample.MVC6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page".L();

            return View();
        }
    }
}
