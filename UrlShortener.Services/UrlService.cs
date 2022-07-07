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
        private readonly string ShortUrlBaseAdress = "https://shortener-test.azurewebsites.net/shortened/";
        private readonly SqlContext _context;
        public UrlService(SqlContext context)
        {
            _context = context;           
        }

        public async Task AddUrlsAuto(string url, int userId)
        {
            string shortUrl = UrlConverter(url);          
            if (String.IsNullOrEmpty(shortUrl)) 
            {
                throw new ArgumentException("Wrong url!");    
            }
           
            var userName = _context.Users.FirstOrDefault(x => x.Id == userId).Name;
            await _context.Urls.AddAsync(new Url()
            {
                LongUrl = url,
                CreateDate = DateTime.Now,
                CreatedBy = userName,
                ShortUrl = ShortUrlBaseAdress + shortUrl
            });
            await _context.SaveChangesAsync();

            var urlId = _context.Urls.FirstOrDefault(x => x.ShortUrl == ShortUrlBaseAdress + shortUrl)?.Id;
            if (_context.UserUrl.Any(x => x.UrlId == urlId && x.UserId == userId))
            {
                throw new ArgumentException("Url already exists in your table!");
            }
            await _context.UserUrl.AddAsync(new UserUrl(userId, (int)urlId));
            await _context.SaveChangesAsync();            
        }

        public async Task AddUrlsByYourself(string url, string shortUrl, int userId)
        {
            if (String.IsNullOrEmpty(shortUrl) || String.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Wrong url!");
            }
         
            var userName = _context.Users.FirstOrDefault(x => x.Id == userId).Name;
            await _context.Urls.AddAsync(new Url()
            {
                LongUrl = url,
                CreateDate = DateTime.Now,
                CreatedBy = userName,
                ShortUrl = ShortUrlBaseAdress + shortUrl
            });
            await _context.SaveChangesAsync();

            var urlId = _context.Urls.FirstOrDefault(x => x.ShortUrl == ShortUrlBaseAdress + shortUrl)?.Id;
            if (_context.UserUrl.Any(x => x.UrlId == urlId && x.UserId == userId))
            {
                throw new ArgumentException("Url already exists in your table!");
            }
            await _context.UserUrl.AddAsync(new UserUrl(userId, (int)urlId));
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

        public HttpStatusCode RemoveUrls(int userId, string shortUrl)
        {
            try
            {
                var url = _context.Urls.FirstOrDefault(x => x.ShortUrl == shortUrl);
                var userUrl = _context.UserUrl.FirstOrDefault(x => x.UrlId == url.Id && x.UserId == userId);

                _context.UserUrl.Remove(userUrl);
                _context.Urls.Remove(url);

                _context.SaveChanges();
                return HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return HttpStatusCode.BadRequest;
            }        
        }

        public string UrlForRedirect(string url) 
        {
            var result = _context.Urls.FirstOrDefault(x => x.ShortUrl == url.Replace("%2F", "/"))?.LongUrl;
            return result;  
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
