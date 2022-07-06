using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Domain.Entities
{
    public class UserUrl
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UrlId { get; set; }
        public UserUrl(int userId, int urlId)
        {
            this.UserId = userId;
            this.UrlId = urlId;
        }
    }
}
