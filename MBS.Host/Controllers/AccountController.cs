namespace MBS.Host.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MBS.Application.Dtos;
using MBS.Application.Services;

/// <summary>
/// Контроллер аккаунтов.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserIdentityService userIdentityService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AccountController"/>.
    /// </summary>
    /// <param name="userIdentityService">Сервис идентификационных данных пользователей.</param>
    public AccountController(IUserIdentityService userIdentityService)
    {
        this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
    }

    /// <summary>
    /// Регистрирует пользователя.
    /// </summary>
    /// <param name="userRegistrationDto">DTO регистрации пользователя.</param>
    /// <returns>Результат регистрации пользователя.</returns>
    [HttpPost("register")]
    [Authorize("create_user")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        try
        {
            // Вызываем метод регистрации из сервиса и передаем DTO
            await this.userIdentityService.RegisterAsync(userRegistrationDto);
            // В случае успеха возвращаем статус 200 (OK) и сообщение
            return this.Ok();
        }
        catch (Exception ex)
        {
            // В случае ошибки возвращаем статус 400 (Bad Request) и сообщение об ошибке
            return this.BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Авторизирует пользователя.
    /// </summary>
    /// <param name="userAuthorizationDto">DTO авторизации пользователя.</param>
    /// <returns>Результат авторизации пользователя.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] UserAuthorizationDto userAuthorizationDto)
    {
        try
        {
            // Вызываем метод авторизации из сервиса и передаем DTO
            var tokenDto = await this.userIdentityService
                .AuthorizeAsync(userAuthorizationDto);
            // В случае успеха возвращаем статус 200 (OK) и данные о токене
            return this.Ok(tokenDto);
        }
        catch (Exception ex)
        {
            // В случае ошибки возвращаем статус 400 (Bad Request) и сообщение об ошибке
            return this.BadRequest(ex.Message);
        }
    }
}