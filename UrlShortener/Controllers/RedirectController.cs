using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }
    }
}
