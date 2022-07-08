using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ShortenedController : ControllerBase
    {
        private readonly UserService _userService;//TODO: Remove what not used
        private readonly UrlService _urlService;
        private readonly IMapper _mapper;

        public ShortenedController(UrlService urlService, IMapper mapper, UserService userService)
        {
            _urlService = urlService;
            _mapper = mapper;
            _userService = userService;
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
        public async Task<IActionResult> SetUrlByYourself(string url, string shortUrl)//TODO: Create request DTO
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
        public IActionResult GetUserUrls()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var urlList = _urlService.GetUserUrls(int.Parse(userId));
            return Ok(urlList);
        }

        [HttpGet("GetUrlInfo")]
        public IActionResult GetUrlInfo(string shortUrl)
        {
            var url = _urlService.GetUrlInfo(shortUrl);
            return Ok(url);
        }

        [HttpDelete("RemoveUrl")]
        public IActionResult RemoveUrl(string shortUrl)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var statusCode = _urlService.RemoveUrls(int.Parse(userId), shortUrl);
            if (statusCode == HttpStatusCode.OK)
            {
                return Ok("Success!");
            }
            else
            {
                return BadRequest("Wrong url!");
            }          
        }

        [HttpGet]
        [Route("{shortUrl}")]
        [AllowAnonymous]
        public IActionResult RedirectUser([FromRoute] string shortUrl)
        {
            var url = _urlService.UrlForRedirect(shortUrl);
            return RedirectToAction("Index", "Redirect", new { url });
        }
    }
}
