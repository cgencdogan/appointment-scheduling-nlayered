using Microsoft.AspNetCore.Identity;
using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Entities.Concrete;
using AppointmentSchedulingNLayered.Common.DataTransferObjects.Authentication;
using AppointmentSchedulingNLayered.Common.Jwt;
using AppointmentSchedulingNLayered.Common.Results;
using AppointmentSchedulingNLayered.Common.Exceptions;

namespace AppointmentSchedulingNLayered.Business.Concrete;
public class AuthService : IAuthService {
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUserTokenService _appUserTokenService;

    public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IUserTokenService appUserTokenService) {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _appUserTokenService = appUserTokenService;
    }
    public async Task<IDataResult<TokenInfo>> LoginAsync(LoginDto loginVM) {
        var signInResult = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, true, false);
        if (!signInResult.Succeeded) {
            throw new LoginFailedException();
        }

        AppUser appUser = await _userManager.FindByNameAsync(loginVM.UserName);
        List<string> appRoles = (await _userManager.GetRolesAsync(appUser)).ToList();
        AppUserToken appUserToken = await _appUserTokenService.GetTokenAsync(appUser, appRoles);
        var tokenInfo = new TokenInfo() { Token = appUserToken.Value, ExpireDate = appUserToken.ExpireDate };
        return new SuccessDataResult<TokenInfo>(tokenInfo, "Giriş başarılı");
    }

    public async Task<IResult> RegisterAsync(RegisterDto registerVM) {
        AppUser appUser = new AppUser {
            UserName = registerVM.UserName,
            Email = registerVM.Email,
            FirstName = registerVM.FirstName,
            LastName = registerVM.LastName,
            CreatedDate = DateTime.UtcNow
        };

        var identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);

        if (!identityResult.Succeeded) {
            throw new RegisterFailedException(errors: identityResult.Errors.Select(e => e.Description).ToList()); ;
        }

        await _userManager.AddToRoleAsync(appUser, "User");

        return new SuccessResult("Kayıt başarılı");
    }
}
