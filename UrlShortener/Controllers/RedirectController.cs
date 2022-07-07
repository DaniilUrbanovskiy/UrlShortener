﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace UrlShortener.Api.Controllers
{
    public class RedirectController : Controller
    {
        public ActionResult Index(string url)
        {
            try
            {
                return Redirect("https://www.youtube.com/");
            }
            catch (System.Exception)
            {
                return BadRequest("Wrong url!");
            }        
        }
    }
}
