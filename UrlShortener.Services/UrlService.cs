using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UrlShortener.DataAccess;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Services
{
    public class UrlService
    {
        private readonly string ShortUrlBaseAdress = AppsettingsProvider.GetJsonAppsettingsFile()["ConnectionStrings:ApiBaseAdress"];
        private readonly UnitOfWork _unitOfWork;
        public UrlService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddUrlsAuto(string originalUrl, int userId)
        {
            var isAdded = await AddExistedUrlIfUserNotHave(originalUrl, userId);
            if (isAdded)
            {
                return;
            }

            string shortUrl = UrlConverter(originalUrl);
            if (String.IsNullOrEmpty(shortUrl))
            {
                throw new ArgumentException("Wrong url!");
            }

            await AddShortenedUrl(originalUrl, userId, shortUrl);
        }

        public async Task AddUrlsByYourself(string originalUrl, string shortUrl, int userId)
        {
            if (String.IsNullOrEmpty(shortUrl) || String.IsNullOrEmpty(originalUrl))
            {
                throw new ArgumentException("Wrong url!");
            }
         
            await AddShortenedUrl(originalUrl, userId, shortUrl);
        }

        public async Task<List<Url>> GetUserUrls(int userId)
        {
            var urlsId = await _unitOfWork.UserUrlRepository.GetUserUrlsId(userId);
            var urls = await _unitOfWork.UrlRepository.GetUrlsById(urlsId);
            return urls;
        }

        public async Task<Url> GetUrlInfo(string shortUrl)
        {
            var urlInfo = await _unitOfWork.UrlRepository.GetByShortUrl(shortUrl);
            return urlInfo;
        }

        public async Task RemoveUrls(int userId, string shortUrl)
        {
            var url = await _unitOfWork.UrlRepository.GetByShortUrl(shortUrl);
            if (url == null)
            {
                throw new ArgumentException("Wrong url");
            }
            var userUrl = await _unitOfWork.UserUrlRepository.GetBy(userId, url.Id);
            await _unitOfWork.UserUrlRepository.Remove(userUrl);

            var isUrlExistInUserUrl = await _unitOfWork.UserUrlRepository.IsExist(url.Id);
            if (!isUrlExistInUserUrl)
            {
                await _unitOfWork.UrlRepository.Remove(url);
            }
        }

        public async Task<string> UrlForRedirect(string url) 
        {
            var result = (await _unitOfWork.UrlRepository.GetByShortUrl(ShortUrlBaseAdress + url))?.LongUrl;
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
               
                return result.Replace("/", DateTime.Now.Second.ToString());
            }
            catch (Exception)
            {
                return null;//TODO: return exeption message
            }                    
        }

        private async Task<bool> AddExistedUrlIfUserNotHave(string originalUrl, int userId)
        {
            var longUrlExist = await _unitOfWork.UrlRepository.IsExist(originalUrl);

            if (longUrlExist)
            {
                var url = await _unitOfWork.UrlRepository.GetByLongUrl(originalUrl);
                var userUrlExist = await _unitOfWork.UserUrlRepository.IsExist(userId, url.Id);

                if (userUrlExist)
                {
                    throw new Exception("Url already exists in your table!");
                }
                await _unitOfWork.UserUrlRepository.Add(new UserUrl(userId, url.Id));
                return true;
            }
            return false;
        }

        private async Task AddShortenedUrl(string originalUrl, int userId, string shortUrl)
        {
            var userName = (await _unitOfWork.UserRepository.GetBy(userId)).Name;
            if (!(await _unitOfWork.UrlRepository.IsExist(originalUrl)))
            {
                await _unitOfWork.UrlRepository.Add(new Url()
                {
                    LongUrl = originalUrl,
                    CreateDate = DateTime.Now,
                    CreatedBy = userName,
                    ShortUrl = ShortUrlBaseAdress + shortUrl
                });
            }

            var url = await _unitOfWork.UrlRepository.GetByLongUrl(originalUrl);
            var userUrlExist = await _unitOfWork.UserUrlRepository.IsExist(userId, url.Id);

            if (userUrlExist)
            {
                throw new ArgumentException("Url already exists in your table!");
            }
            await _unitOfWork.UserUrlRepository.Add(new UserUrl(userId, url.Id));
        }

    }
}
