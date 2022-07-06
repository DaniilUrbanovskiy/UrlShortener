using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> Registr([FromBody] UserRegisterRequest userInfo)
        {
            try
            {
                var user = _mapper.Map<User>(userInfo);
                await _userService.Registration(user);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userInfo)
        {
            try
            {
                var user = await _userService.Login(userInfo.Login, userInfo.Password);
                var token = JwtHealper.CreateToken(user);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
