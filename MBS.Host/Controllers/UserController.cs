using MBS.Host.ApplicationServices;
using MBS.Host.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Host.Controllers;

[Route("api")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("users/available")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAvailableUsers()
    {
        var username = this.User.Identity!.Name;
        var users = await userService.GetAvailableUsersAsync(username);
        return Ok(users);
    }

    [HttpGet("users/current")]
    public async Task<IActionResult> GetCurrentUser(string username)
    {
        var user = await userService.GetUserAsync(username);
        if (user == null)
        {
            return NotFound("Пользователь не найден.");
        }
        return Ok(user);
    }


    [HttpPut("user")]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UserUpdateDto updateUserDto)
    {
        var username = this.User.Identity!.Name;
        try
        {
            await userService.UpdateUserAsync(username, updateUserDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("user/avatar")]
    public async Task<IActionResult> UpdateUserAvatar(
        [FromForm] IFormFile avatarFile)
    {
        var username = this.User.Identity!.Name;
        try
        {
            await userService.UpdateUserAvatarAsync(username, avatarFile);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}