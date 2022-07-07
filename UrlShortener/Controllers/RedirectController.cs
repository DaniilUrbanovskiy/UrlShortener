using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Api.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult Index(string url)
        {
            try
            {
                return Redirect(url);
            }
            catch (System.Exception)
            {
                return BadRequest("Wrong url!");
            }        
        }
    }
}
