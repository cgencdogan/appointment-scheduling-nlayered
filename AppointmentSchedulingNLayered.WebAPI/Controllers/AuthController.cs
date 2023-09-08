using Microsoft.AspNetCore.Mvc;
using AppointmentSchedulingNLayered.Business.Abstract;
using AppointmentSchedulingNLayered.Common.DataTransferObjects.Authentication;

namespace AppointmentSchedulingNLayered.WebAPI.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase {

    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto) {
        var result = await _authService.RegisterAsync(registerDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto) {
        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }
}