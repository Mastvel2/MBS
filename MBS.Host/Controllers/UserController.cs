using MBS.Host.ApplicationServices;
using MBS.Host.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var user = await userService.GetUserAsync(username);
        if (user == null)
        {
            return NotFound("Пользователь не найден.");
        }
        return Ok(user);
    }


    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateUser(string username,
        [FromBody] UserUpdateDto updateUserDto)
    {
        try
        {
            await userService.UpdateUserAsync(username, updateUserDto);
            return Ok("Информация успешно обновлена.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{username}/avatar")]
    public async Task<IActionResult> UpdateUserAvatar(string username, [FromForm] IFormFile avatarFile)
    {
        try
        {
            await userService.UpdateUserAvatarAsync(username, avatarFile);
            return Ok("Аватар пользовтеля успешно обновлён.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("{username}/status")]
    public async Task<IActionResult> UpdateUserStatus(string username, [FromBody] string status)
    {
        try
        {
            await userService.UpdateStatus(username, status);
            return Ok("User status updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{username}/last-login-time")]
    public async Task<IActionResult> UpdateLastLoginTime(string username, [FromBody] DateTime lastLoginTime)
    {
        try
        {
            await userService.UpdateLastLoginTime(username, lastLoginTime);
            return Ok("User last login time updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}