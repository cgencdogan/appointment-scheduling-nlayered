using AppointmentSchedulingNLayered.Business.Concrete;
using AppointmentSchedulingNLayered.Common.Jwt;
using AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
using AppointmentSchedulingNLayered.Entities.Concrete;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace AppointmentSchedulingNLayered.Tests.Services;

public class UserTokenServiceTests {
    private readonly Mock<IAppUserTokenRepository> _appUserTokenRepoMock;
    private readonly UserTokenService _sut;

    public UserTokenServiceTests() {
        _appUserTokenRepoMock = new Mock<IAppUserTokenRepository>();
        var jwtOptions = Options.Create(new JwtSettings() { Audience = "TestAudience", ExpiryMinutes = 1, Issuer = "TestIssuer", Secret = "TestSecretKeySuperSecretTestKeyDummyKey" });
        _sut = new UserTokenService(_appUserTokenRepoMock.Object, jwtOptions);
    }

    [Fact]
    public async Task GetCountByUserIdAsync_ShouldReturnInt_WhenCalledSuccessfully() {
        _appUserTokenRepoMock.Setup(a => a.GetCountByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<int>());

        var result = await _sut.GetCountByUserIdAsync(It.IsAny<Guid>());

        result.Should().BeOfType(typeof(int));
    }

    [Fact]
    public async Task GetTokenByUserIdAsync_ShouldReturnAppUserToken_WhenCalledSuccessfully() {
        _appUserTokenRepoMock.Setup(a => a.GetTokenByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(new AppUserToken());

        var result = await _sut.GetTokenByUserIdAsync(It.IsAny<Guid>());

        result.Should().BeOfType(typeof(AppUserToken));
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GenerateTokenAsync_ShouldReturnTokenInfo_WhenCalledSuccessfully() {
        var appUser = new AppUser() {
            Id = Guid.NewGuid(),
            UserName = "TestUsername",
            Email = "Test@Mail.com"
        };

        var result = await _sut.GenerateTokenAsync(appUser, new List<string>());

        result.Should().BeOfType(typeof(TokenInfo));
        result.Should().NotBeNull();
        result.Token.Should().NotBeNull().And.NotBeEmpty();
    }

    [Fact]
    public async Task GetTokenAsync_ShouldReturnAppUserToken_WhenCalledSuccessfully() {
        var appUser = new AppUser() {
            Id = Guid.NewGuid(),
            UserName = "TestUsername",
            Email = "Test@Mail.com"
        };

        var result = await _sut.GetTokenAsync(appUser, new List<string>());

        result.Should().BeOfType(typeof(AppUserToken));
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull().And.NotBeEmpty();
    }
}