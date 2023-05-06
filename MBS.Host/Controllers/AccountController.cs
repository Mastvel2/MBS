using MBS.Host.ApplicationServices;
using MBS.Host.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Класс контроллера для работы с аккаунтами пользователей
    public class AccountController : ControllerBase
    {
        private readonly IUserIdentityService userIdentityService;

        // Конструктор, принимает сервис IUserIdentityService
        public AccountController(IUserIdentityService userIdentityService)
        {
            this.userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        }

        // Метод для регистрации пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
        {
            try
            {
                // Вызываем метод регистрации из сервиса и передаем DTO
                await userIdentityService.RegisterAsync(userRegistrationDto);
                // В случае успеха возвращаем статус 200 (OK) и сообщение
                return Ok("User successfully registered.");
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус 400 (Bad Request) и сообщение об ошибке
                return BadRequest(ex.Message);
            }
        }

        // Метод для авторизации пользователя
        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(
            [FromBody] UserAuthorizationDto userAuthorizationDto)
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
}