using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult Index(string url)
        {
            return Redirect(url);
        }
    }
}
