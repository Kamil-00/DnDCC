using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Projekt.Services.Interfaces;
using Projekt.ViewModel.VM;

namespace Projekt.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserVm vm)
    {
        var (success, error, result) = await _auth.RegisterAsync(vm, "User");
        if (!success) return BadRequest(new { error });

        // Set token in HttpOnly cookie
        Response.Cookies.Append("access_token", result!.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,             // HTTPS
            SameSite = SameSiteMode.Lax,
            Expires = result.ExpiresAt
        });

        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserVm vm)
    {
        var (success, error, result) = await _auth.LoginAsync(vm);
        if (!success) return Unauthorized(new { error });

        Response.Cookies.Append("access_token", result!.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = result.ExpiresAt
        });

        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return NoContent();
    }
}