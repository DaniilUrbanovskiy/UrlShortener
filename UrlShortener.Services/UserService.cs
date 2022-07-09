using System;
using System.Threading.Tasks;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Registration(User user)
        {
            var isExist = await _userRepository.IsExist(user.Login);

            if (isExist == true)
            {
                throw new Exception("This login already exists!");
            }
            await _userRepository.Add(user);
        }

        public async Task<User> Login(User user)
        {
            var response = await _userRepository.Get(user);

            if (response == null)
            {
                throw new Exception("Invalid login or password");
            }
            return response;
        }
    }
}
