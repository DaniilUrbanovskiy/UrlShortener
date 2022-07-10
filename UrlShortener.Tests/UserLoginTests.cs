using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using UrlShortener.DataAccess;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;
using FluentAssertions;

namespace UrlShortener.Tests
{
    [TestClass]
    public class UserLoginTests
    {
        private readonly UserService _userService;

        public UserLoginTests()
        {
            var options = new DbContextOptionsBuilder<SqlContext>().UseInMemoryDatabase(databaseName: "UrlShortener").Options;
            _userService = new UserService(new UserRepository(new SqlContext(options)));
        }

        private User TestUser => new User
        {
            Id = 1,
            Name = "Danik2",
            Birthday = new DateTime(2010, 10, 10),
            Login = "Danik2",
            Password = "Danik2001",
            Email = "Danik@gmail.com"
        };

        [TestMethod]
        [Priority(0)]
        public async Task TC_1_UserLoginWithCorrectCredentials_ShouldReturnCreatedUser()
        {
            await _userService.Registration(TestUser);
            var result = await _userService.Login(new User() 
            {
                Login = TestUser.Login,
                Password = TestUser.Password
            });
            TestUser.Should().BeEquivalentTo(result);
        }

        [TestMethod]
        [Priority(1)]
        public async Task TC_2_UserLoginWithIncorrectLogin_ShouldReturnError()
        {
            try
            {
                var result = await _userService.Login(new User()
                {
                    Login = "777",
                    Password = TestUser.Password
                });
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        [Priority(2)]
        public async Task TC_3_UserLoginWithIncorrectPassword_ShouldReturnError()
        {
            try
            {
                var result = await _userService.Login(new User()
                {
                    Login = TestUser.Login,
                    Password = "777"
                });
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
