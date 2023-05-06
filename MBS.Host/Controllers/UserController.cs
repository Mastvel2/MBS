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
            return NotFound("User not found.");
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
            return Ok("User information updated successfully.");
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
            // await userService.UpdateUserAvatarAsync(username, avatarFile);
            return Ok("User avatar updated successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}