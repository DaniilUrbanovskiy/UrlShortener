using UrlShortener.DataAccess.Repository;

namespace UrlShortener.DataAccess
{
    public class UnitOfWork
    {
        public UnitOfWork(UrlRepository urlRepository, UserRepository userRepository, UserUrlRepository userUrlRepository)
        {
            UrlRepository = urlRepository;
            UserRepository = userRepository;
            UserUrlRepository = userUrlRepository;
        }

        public UrlRepository UrlRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public UserUrlRepository UserUrlRepository { get; set; }
    }
}
