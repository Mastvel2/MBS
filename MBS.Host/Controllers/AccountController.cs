using Microsoft.AspNetCore.Mvc;
using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Dtos;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    // Здесь должен быть ваш код для работы с репозиторием или службой пользователей
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(IUserRepository userRepository, IUserService? userService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        // Проверьте существующих пользователей с таким же адресом электронной почты
        var existingUser = await _userRepository.GetByNameAsync(userDto.Email);
        if (existingUser != null)
        {
            return BadRequest("Пользователь с таким логином уже существует.");
        }
        // Создайте нового пользователя
        var user = new User
        {
            Email = userDto.Email
        };
        string passwordSalt = Password.GenerateSalt();
        string passwordHash = Password.GenerateHash(userDto.Password, passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        // Сохраните нового пользователя в базе данных
        await _userRepository.AddAsync(user);

        // Вернуть успешный результат с идентификатором пользователя
        return Ok(new { Id = user.Id });
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        // Аутентификация пользователя
        var user = await _userRepository.GetByNameAsync(userDto.Email);
        var isPasswordValid = Password.VerifyPassword(userDto.Password, user.PasswordHash, user.PasswordSalt);

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        var JWTtoken = _userService.GenerateJwtToken(user);

        // Вернуть успешный результат с JWT
        return Ok(new { Token = JWTtoken });
        /*  // Реализуем логику проверки пользователя и пароля
        // Если пользователь найден и пароль верный, генерируется JWT токен

        
    }
}