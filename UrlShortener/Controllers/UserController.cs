using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UrlShortener.Api.Dto.Model;
using UrlShortener.Api.Dto.Requests;
using UrlShortener.Api.Dto.Responses;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("registr")]
        public async Task<IActionResult> Registr([FromBody] UserRegistrRequest userInfo)
        {
            try
            {
                var user = _mapper.Map<User>(userInfo);
                await _userService.Registration(user);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(new WebServiceError(System.Net.HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userInfo)
        {
            try
            {
                var user = _mapper.Map<User>(userInfo);
                user = await _userService.Login(user);
                var tokenResponse = JwtHealper.CreateToken(user);
                var token = new TokenResponse(new JwtSecurityTokenHandler().WriteToken(tokenResponse));

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new WebServiceError(System.Net.HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}
