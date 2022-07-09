using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Dto.Requests
{
    public class UserRegistrRequest : IValidatableObject
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Incorrewct datetomne formday")]
        public DateTime Birthday { get; set; }
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Birthday > DateTime.Now)
            {
                yield return new ValidationResult("Birthday cant be in feature");
            }
            if (!Email.Contains("@"))
            {
                yield return new ValidationResult("Incorrect rmail");
            }
        }
    }
}
