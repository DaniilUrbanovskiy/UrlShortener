using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UrlShortener.DataAccess;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Services
{
    public class UrlService
    {
        private readonly string ShortUrlBaseAdress = "http://localhost:44376/shortened/";
        private readonly SqlContext _context;
        public UrlService(SqlContext context)
        {
            _context = context;           
        }

        public async Task AddUrlsAuto(string url, User user)
        {
            string shortUrl = UrlConverter(url);          
            if (String.IsNullOrEmpty(shortUrl)) 
            {
                throw new ArgumentException("Wrong url!");    
            }

            var urlId = _context.Urls.FirstOrDefault(x => x.ShortUrl == shortUrl)?.Id;
            if (_context.UserUrl.Any(x => x.UrlId == urlId && x.UserId == user.Id))
            {
                throw new ArgumentException("Url already exists in your table!");
            }

            await _context.Urls.AddAsync(new Url()
            {
                LongUrl = url,
                CreateDate = DateTime.Now,
                CreatedBy = user.Name,
                ShortUrl = ShortUrlBaseAdress + shortUrl
            });            
            await _context.UserUrl.AddAsync(new UserUrl(user.Id, (int)urlId));
            await _context.SaveChangesAsync();            
        }

        public async Task AddUrlsByYourself(string url, string shortUrl, User user)
        {
            if (String.IsNullOrEmpty(shortUrl) || String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Wrong url!");
            }

            var urlId = _context.Urls.FirstOrDefault(x => x.ShortUrl == shortUrl)?.Id;
            if (_context.UserUrl.Any(x => x.UrlId == urlId && x.UserId == user.Id))
            {
                throw new ArgumentException("Url already exists in your table!");
            }

            await _context.Urls.AddAsync(new Url()
            {
                LongUrl = url,
                CreateDate = DateTime.Now,
                CreatedBy = user.Name,
                ShortUrl = ShortUrlBaseAdress + shortUrl
            });
            await _context.UserUrl.AddAsync(new UserUrl(user.Id, (int)urlId));
            await _context.SaveChangesAsync();
        }

        public List<Url> GetUserUrls(int userId)
        {
            var urlIds = _context.UserUrl.Where(x => x.UserId == userId).Select(x => x.UrlId).ToList();
            var urls = _context.Urls.Where(x => urlIds.Contains(x.Id)).ToList();
            return urls;
        }

        public Url GetUrlInfo(string shortUrl)
        {
            var urlInfo = _context.Urls.FirstOrDefault(x => x.ShortUrl == shortUrl);
            return urlInfo;
        }

        public void RemoveUrls(int userId, string shortUrl)
        {
            var urlId = _context.Urls.FirstOrDefault(x => x.ShortUrl == shortUrl)?.Id;

            var url = _context.UserUrl.FirstOrDefault(x => x.UrlId == urlId && x.UserId == userId);
            _context.UserUrl.Remove(url);
            _context.SaveChanges();
        }

        public string UrlForRedirect(string url) 
        {
            return _context.Urls.FirstOrDefault(x => x.ShortUrl == ShortUrlBaseAdress + url).LongUrl;
        }

        private string UrlConverter(string url) 
        {
            Random random = new Random();
            try
            {
                char[] urlElementsArray = (url + url).ToCharArray();
                string result = string.Empty;
                for (int i = 0; i < 5; i++)
                {
                    result = result + urlElementsArray[urlElementsArray.Length - 1 - i].ToString() + random.Next(0, 10);
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }                    
        }
    }
}
