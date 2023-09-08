using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Business.Concrete;
using AppointmentSchedulingNLayered.Common.DataTransferObjects.Authentication;
using AppointmentSchedulingNLayered.Common.Exceptions;
using AppointmentSchedulingNLayered.Common.Jwt;
using AppointmentSchedulingNLayered.Entities.Concrete;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AppointmentSchedulingNLayered.Tests.Services;
public class AuthServiceTests {
    private readonly AuthService _sut;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IUserClaimsPrincipalFactory<AppUser>> _userPrincipalFactoryMock;
    private readonly Mock<RoleManager<AppRole>> _roleManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<IUserTokenService> _appUserTokenServiceMock;

    public AuthServiceTests() {
        _userManagerMock = new Mock<UserManager<AppUser>>(new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        _roleManagerMock = new Mock<RoleManager<AppRole>>(new Mock<IRoleStore<AppRole>>().Object, null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<AppUser>>(_userManagerMock.Object, _contextAccessorMock.Object, _userPrincipalFactoryMock.Object, null, null, null, null);
        _appUserTokenServiceMock = new();
        _sut = new AuthService(_signInManagerMock.Object, _userManagerMock.Object, _roleManagerMock.Object, _appUserTokenServiceMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowLoginFailedException_WhenSignInFailed() {
        _signInManagerMock.Setup(m => m.PasswordSignInAsync("TestUsername", "TestPassword", It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Failed);

        Func<Task> act = () => _sut.LoginAsync(new LoginDto { UserName = "TestUsername", Password = "TestPassword" });

        await act.Should().ThrowAsync<LoginFailedException>();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokenInfo_WhenSignInSuccessfully() {
        _signInManagerMock.Setup(m => m.PasswordSignInAsync("TestUsername", "TestPassword", It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);
        _userManagerMock.Setup(u => u.FindByNameAsync("TestUsername")).ReturnsAsync(new AppUser());
        _userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<AppUser>())).ReturnsAsync(new List<string>());
        _appUserTokenServiceMock.Setup(a => a.GetTokenAsync(It.IsAny<AppUser>(), It.IsAny<List<string>>())).ReturnsAsync(new AppUserToken());

        var result = await _sut.LoginAsync(new LoginDto { UserName = "TestUsername", Password = "TestPassword" });

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().BeOfType<TokenInfo>();
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowRegisterFailedException_WhenRegisterFailed() {
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

        Func<Task> act = () => _sut.RegisterAsync(new RegisterDto());

        await act.Should().ThrowAsync<RegisterFailedException>();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenRegisterSuccessfully() {
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var result = await _sut.RegisterAsync(new RegisterDto());

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }
}