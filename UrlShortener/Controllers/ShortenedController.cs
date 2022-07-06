﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortenedController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UrlService _urlService;
        private readonly IMapper _mapper;

        public ShortenedController(UrlService urlService, IMapper mapper, UserService userService)
        {
            _urlService = urlService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
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

        [HttpPost]
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

        [HttpGet]
        public IActionResult GetUserUrls()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var urlList = _urlService.GetUserUrls(int.Parse(userId));
            return Ok(urlList);
        }

        [HttpGet]
        public IActionResult GetUrlInfo(string shortUrl) 
        {
            var url = _urlService.GetUrlInfo(shortUrl);
            return Ok(url);
        }

        [HttpDelete]
        public IActionResult RemoveUrl(string shortUrl) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _urlService.RemoveUrls(int.Parse(userId), shortUrl);
            return Ok();
        }

        [HttpPost]
        public override RedirectResult Redirect([FromQuery]string shortUrl) 
        {
            var url = _urlService.UrlForRedirect(shortUrl);
            return RedirectPermanent(url);
        }
    }
}