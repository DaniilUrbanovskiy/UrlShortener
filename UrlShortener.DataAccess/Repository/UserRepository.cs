using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess.Repository
{
    public class UserRepository
    {
        private readonly SqlContext _context;
        public UserRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        public async Task<User> Get(User user)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login && u.Password == user.Password);
        }

        public async Task<User> GetBy(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
