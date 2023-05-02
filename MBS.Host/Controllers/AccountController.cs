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
        private readonly IUserIdentityService _userIdentityService;

        // Конструктор, принимает сервис IUserIdentityService
        public AccountController(IUserIdentityService userIdentityService)
        {
            _userIdentityService = userIdentityService ?? throw new ArgumentNullException(nameof(userIdentityService));
        }

        // Метод для регистрации пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
        {
            try
            {
                // Вызываем метод регистрации из сервиса и передаем DTO
                await _userIdentityService.RegisterAsync(userRegistrationDto);
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
        public async Task<IActionResult> Login([FromBody] UserAuthorizationDto userAuthorizationDto)
        {
            try
            {
                // Вызываем метод авторизации из сервиса и передаем DTO
                var tokenDto = await _userIdentityService.AuthorizeAsync(userAuthorizationDto);
                // В случае успеха возвращаем статус 200 (OK) и данные о токене
                return Ok(tokenDto);
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус 400 (Bad Request) и сообщение об ошибке
                return BadRequest(ex.Message);
            }
        }
    }
}