using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.DataAccess;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Services
{
    public class UserService
    {
        private readonly SqlContext _context;
        public UserService(SqlContext context)
        {
            _context = context;
        }

        public Task Registration(User user)
        {
            var isExist = _context.Users.Any(u => u.Login == user.Login);

            if (isExist == true)
            {
                throw new Exception("This login already exists!");
            }
            _context.Add(user);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<User> Login(string login, string password)
        {
            var user = _context.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Invalid login or password");
            }
            return Task.FromResult(user);
        }
    }
}
