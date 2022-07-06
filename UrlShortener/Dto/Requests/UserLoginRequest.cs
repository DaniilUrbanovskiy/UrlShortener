using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Dto.Requests
{
    public class UserLoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
