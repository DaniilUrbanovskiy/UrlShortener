using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess.Repository
{
    public class UrlRepository
    {
        private readonly SqlContext _context;
        public UrlRepository(SqlContext context)
        {
            _context = context;
        }
        
        public async Task Add(Url url)
        {
            await _context.Urls.AddAsync(url);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Url url)
        {
            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(string longUrl)
        {
            var a = await _context.Urls.AnyAsync(x => x.LongUrl == longUrl);
            return a;
        }

        public async Task<Url> GetByShortUrl(string url)
        {
            return await _context.Urls.FirstOrDefaultAsync(x => x.ShortUrl == url);
        }

        public async Task<Url> GetByLongUrl(string url)
        {
            return await _context.Urls.FirstOrDefaultAsync(x => x.LongUrl == url);
        }

        public async Task<List<Url>> GetUrlsById(List<int> urlsId)
        {
            return await _context.Urls.Where(x => urlsId.Contains(x.Id)).ToListAsync();
        }
    }
}
