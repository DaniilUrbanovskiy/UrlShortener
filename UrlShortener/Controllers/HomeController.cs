using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("http://www.google.com");
        }
    }
}
