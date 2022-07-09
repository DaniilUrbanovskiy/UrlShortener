using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ShortenedController : ControllerBase
    {
        private readonly UrlService _urlService;

        public ShortenedController(UrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost("SetAuto")]
        public async Task<IActionResult> SetUrlAuto(string url)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                await _urlService.AddUrlsAuto(url, int.Parse(userId));
                return Ok("Url added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SetByYourself")]
        public async Task<IActionResult> SetUrlByYourself(string url, string shortUrl)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                await _urlService.AddUrlsByYourself(url, shortUrl, int.Parse(userId));
                return Ok("Url added!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUrls")]
        public async Task<IActionResult> GetUserUrls()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var urlList = await _urlService.GetUserUrls(int.Parse(userId));
            return Ok(urlList);
        }

        [HttpGet("GetUrlInfo")]
        public async Task<IActionResult> GetUrlInfo(string shortUrl)
        {
            var url = await _urlService.GetUrlInfo(shortUrl);
            return Ok(url);
        }

        [HttpDelete("RemoveUrl")]
        public async Task<IActionResult> RemoveUrl(string shortUrl)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _urlService.RemoveUrls(int.Parse(userId), shortUrl);
                return Ok("Success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{shortUrl}")]
        [AllowAnonymous]
        public async Task<IActionResult> RedirectUser([FromRoute] string shortUrl)
        {
            var url = await _urlService.UrlForRedirect(shortUrl);
            return RedirectToAction("Index", "Redirect", new { url });
        }
    }
}
