namespace MBS.Host.Controllers;

using MBS.Application.Dtos;
using MBS.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер пользователей.
/// </summary>
[Route("api")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserController"/>.
    /// </summary>
    /// <param name="userService">Сервис пользователей.</param>
    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    /// <summary>
    /// Получает список доступных пользователей.
    /// </summary>
    /// <returns>Пользователи.</returns>
    [HttpGet("users/available")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAvailableUsers()
    {
        var username = this.User.Identity!.Name;
        var users = await this.userService.GetAvailableUsersAsync(username);
        return this.Ok(users);
    }

    /// <summary>
    /// Получает информацию о текущем пользователе.
    /// </summary>
    /// <returns>Пользователь.</returns>
    [HttpGet("users/current")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(string username)
    {
        try
        {
            var user = await this.userService.GetUserAsync(username);
            return this.Ok(user);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Обновляет пользователя.
    /// </summary>
    /// <param name="updateUserDto">DTO обновления пользователя.</param>
    /// <returns>Результат обновления пользователя.</returns>
    [HttpPut("user")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto updateUserDto)
    {
        var username = this.User.Identity!.Name;
        try
        {
            await this.userService.UpdateUserAsync(username, updateUserDto);
            return this.Ok();
        }
        catch (Exception ex)
        {
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновляет аватар пользователя.
    /// </summary>
    /// <param name="formCollection">Коллекция данных формы.</param>
    /// <returns>Результат обновления аватара.</returns>
    [HttpPost("user/avatar")]
    public async Task<IActionResult> UpdateUserAvatar([FromForm] IFormCollection formCollection)
    {
        var username = this.User.Identity!.Name;
        try
        {
            var file = formCollection.Files.SingleOrDefault();
            if (file == null)
            {
                throw new Exception("Необходимо выбрать один файл аватара");
            }

            await using var fs = file.OpenReadStream();
            await this.userService.UpdateUserAvatarAsync(username, file.FileName, fs);
            return this.Ok();
        }
        catch (Exception ex)
        {
            return this.BadRequest(ex.Message);
        }
    }
}