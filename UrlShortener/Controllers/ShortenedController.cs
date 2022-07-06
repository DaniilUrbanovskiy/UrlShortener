using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortenedController : ControllerBase
    {
        private readonly UrlService _urlService;

        public ShortenedController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost]
        public async Task<IActionResult> SetUrl(string url) 
        {
            try
            {
                await _urlService.AddUrlsAuto(url, "userName");
                return Ok("Url added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }
    }
}
