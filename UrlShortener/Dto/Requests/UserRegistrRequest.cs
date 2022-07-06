using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Dto.Requests
{
    public class UserRegistrRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
    }
}
