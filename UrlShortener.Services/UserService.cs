using Microsoft.EntityFrameworkCore;
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

        public async Task Registration(User user)
        {
            var isExist = _context.Users.Any(u => u.Login == user.Login);

            if (isExist == true)
            {
                throw new Exception("This login already exists!");
            }
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> Login(string login, string password)
        {
            var user = await _context.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("Invalid login or password");
            }
            return user;
        }
    }
}
