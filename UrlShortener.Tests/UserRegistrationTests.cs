using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortener.DataAccess;
using UrlShortener.DataAccess.Repository;
using UrlShortener.Domain.Entities;
using UrlShortener.Services;

namespace UrlShortener.Tests
{
    [TestClass]
    public class UserRegistrationTests
    {
        private UserService _userService;

        public UserRegistrationTests()
        {
            var options = new DbContextOptionsBuilder<SqlContext>().UseInMemoryDatabase(databaseName: "UrlShortener").Options;
            _userService = new UserService(new UserRepository(new SqlContext(options)));
        }

        [TestMethod]
        [Priority(0)]
        public async Task TC_1_RegisterAndAddNewUserToDataBase_ShouldReturnSuccess()
        {
            var testData = new User()
            {
                Name = "Danik",
                Birthday = DateTime.Now,
                Login = "Danik",
                Password = "Danik2001",
                Email = "Danik@gmail.com"
            };

            try
            {
                await _userService.Registration(testData);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        [Priority(1)]
        public async Task TC_2_RegisterAndAddExistedUserToDataBase_ShouldReturnError()
        {
            var testData = new User()
            {
                Name = "Danik",
                Birthday = DateTime.Now,
                Login = "Danik",
                Password = "Danik2001",
                Email = "Danik@gmail.com"
            };

            try
            {
                await _userService.Registration(testData);
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [DataTestMethod]
        [Priority(2)]
        [DynamicData(nameof(TestUserData), DynamicDataSourceType.Method)]
        public async Task TC_3_RegisterAndAddUserWithWrongDataToDataBase_ShouldReturnError(User testData)
        {
            try
            {
                await _userService.Registration(testData);
                Assert.IsTrue(false);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        private static IEnumerable<object[]> TestUserData()
        {
            return new List<object[]>()
            {
                new object[] {new User()//Incorrect email
                {
                     Name = "Danik",
                     Birthday = DateTime.Now,
                     Login = "Danik",
                     Password = "Danik2001",
                     Email = "" }
                },

                new object[] {new User()//Incorrect birthday
                {
                     Name = "Danik",
                     Birthday = DateTime.Now.AddDays(1),
                     Login = "Danik",
                     Password = "Danik2001",
                     Email = "Danik@gmail.com" }
                },

                new object[] {new User()//Incorrect password
                {
                     Name = "Danik",
                     Birthday = DateTime.Now,
                     Login = "Danik",
                     Password = "1",
                     Email = "Danik@gmail.com" }
                },
            };
        }
    }
}
    