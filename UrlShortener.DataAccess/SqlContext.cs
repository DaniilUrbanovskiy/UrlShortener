using Microsoft.EntityFrameworkCore;
using System;
using UrlShortener.Domain.Entities;

namespace UrlShortener.DataAccess
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<UserUrl> UserUrl { get; set; }
    }
}
