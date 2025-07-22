using System.Security.Claims;
using Backend.DTO;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    
    [HttpPost]
    public async Task<ActionResult> login([FromBody] UserLoginDTO userLoginDto)
    {
        User userToAuthenticate = new User();
        userToAuthenticate.Email = userLoginDto.Email;
        userToAuthenticate.Password = userLoginDto.Password;
        User? authenticatedUser = await _userService.Authenticate(userToAuthenticate);

        ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Role, authenticatedUser?.Role ?? string.Empty));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authenticatedUser?.Id.ToString() ?? string.Empty));
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        return Ok("Logged in successfully!");
    }
    
    [HttpPost]
    [Authorize(Roles =("Admin"))]
    public async Task<ActionResult> register([FromBody] UserDTO userDto)
    {
        AuthenticateResult result = await HttpContext.AuthenticateAsync();
        if (result.Succeeded)
        {
            ClaimsIdentity? identity = result.Principal.Identity as ClaimsIdentity;
            string? userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            User user = new User(userDto.Name, userDto.Surname, userDto.Email, userDto.Password, "User",await _userService.Get(Guid.Parse(userId)));
            await _userService.CreateUser(user);
            return Ok("Registration successful!");
        }
        
        return Forbid("Authentication error!");
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<string>> logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("Logged out successfully!");
    }
}