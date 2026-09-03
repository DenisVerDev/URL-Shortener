using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Text;
using URL_Shortener.Data.Models;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Services;
using bc = BCrypt.Net.BCrypt;

namespace URL_Shortener.Tests.ServicesTests.UserVerificationServiceTests
{
    public class UVSTests
    {
        private IUsersRepository _ur;
        private IUserVerification _uv;

        private const string _login = "denver";
        private const string _password = "admin123";

        private User _user;

        public UVSTests()
        {
            _user = new User
            {
                Id = 1,
                Login = _login,
                PasswordHash = bc.HashPassword(_password),
                RegistrationDate = DateTime.UtcNow,
                RoleId = 1
            };

            _ur = A.Fake<IUsersRepository>();
            A.CallTo(() => _ur.FindUserAsync(A<string>._)).Returns((User?)null);
            A.CallTo(() => _ur.FindUserAsync(_user.Login)).Returns(_user);

            _uv = new UserVerificationService(_ur);
        }

        [Fact]
        public async Task VerifyUserAsync_AccurateLoginAndPassword_ReturnsSuccess()
        {
            // Act
            var result = await _uv.VerifyUserAsync(_login, _password);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.Equal(UserVerificationResultCode.Success, result.Status);
        }

        [Fact]
        public async Task VerifyUserAsync_AccurateLoginAndPassword_ReturnsUnchangedRealUser()
        {
            // Act
            var result = await _uv.VerifyUserAsync(_login, _password);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.User);

            Assert.Equal(_user.Id, result.User.Id);
            Assert.Equal(_user.Login, result.User.Login);
            Assert.Equal(_user.PasswordHash, result.User.PasswordHash);
            Assert.Equal(_user.RegistrationDate, result.User.RegistrationDate);
            Assert.Equal(_user.RoleId, result.User.RoleId);
        }

        [Fact]
        public async Task VerifyUserAsync_WrongLogin_ReturnsAbsentUser()
        {
            // Act
            var result = await _uv.VerifyUserAsync("abrakadbra", _password);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.User);
            Assert.Equal(UserVerificationResultCode.AbsentUser, result.Status);
        }

        [Fact]
        public async Task VerifyUserAsync_WrongPassword_ReturnsVerificationFailure()
        {
            // Act
            var result = await _uv.VerifyUserAsync(_login, "12345678");

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.User);
            Assert.Equal(UserVerificationResultCode.VerificationFailure, result.Status);
        }

        [Theory]
        [InlineData(null, _password)]
        [InlineData("", _password)]
        [InlineData(_login, null)]
        [InlineData(_login, "")]
        public async Task VerifyUserAsync_NullOrEmptyLoginAndPassword_ThrowsArgumentException(string? login, string? password)
        {
            // Act
            var exception = await Record.ExceptionAsync(() => _uv.VerifyUserAsync(login, password));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }
}
