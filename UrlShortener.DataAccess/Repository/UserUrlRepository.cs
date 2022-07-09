using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess.Repository
{
    public class UserUrlRepository
    {
        private readonly SqlContext _context;
        public UserUrlRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task Add(UserUrl userUrl)
        {
            await _context.UserUrl.AddAsync(userUrl);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(UserUrl userUrl)
        {
            _context.UserUrl.Remove(userUrl);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int userId, int urlId)
        {
            return await _context.UserUrl.AnyAsync(x => x.UrlId == urlId && x.UserId == userId);
        }

        public async Task<bool> IsExist(int urlId)
        {
            return await _context.UserUrl.AnyAsync(x => x.UrlId == urlId);
        }

        public async Task<List<int>> GetUserUrlsId(int userId)
        {
            return await _context.UserUrl.Where(x => x.UserId == userId).Select(x => x.UrlId).ToListAsync();
        }

        public async Task<UserUrl> GetBy(int userId, int urlId)
        {
            return await _context.UserUrl.FirstOrDefaultAsync(x => x.UrlId == urlId && x.UserId == userId);
        }
    }
}
