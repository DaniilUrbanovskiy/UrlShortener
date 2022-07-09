using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain.Entities
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
