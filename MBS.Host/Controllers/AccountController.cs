using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MBS.Domain;
using MBS.Domain.Services;
using MBS.Host.Dtos;
using MBS.Host.Repositories;
using WebMessenger.Security;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    // Здесь должен быть ваш код для работы с репозиторием или службой пользователей
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
        //_passwordUserSecurity = passwordUserSecurity;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        // Проверьте существующих пользователей с таким же адресом электронной почты
        var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            return BadRequest("Пользователь с таким логином уже существует.");
        }
        // Создайте нового пользователя
        var user = new User
        {
            Email = userDto.Email
        };
        string passwordSalt = PasswordUserSecurity.GenerateSalt();
        string passwordHash = PasswordUserSecurity.GenerateHash(userDto.Password, passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        // Сохраните нового пользователя в базе данных
        await _userRepository.AddAsync(user);

        // Вернуть успешный результат с идентификатором пользователя
        return Ok(new { Id = user.Id });
        
       /* try
        {
            await _userService.CreateAsync(user, userDto.Password);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }*/
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto userDto)
    {
        // Аутентификация пользователя
        var user = await _userRepository.GetByEmailAsync(userDto.Email);
        var isPasswordValid = PasswordUserSecurity.VerifyPassword(userDto.Password, user.PasswordHash, user.PasswordSalt);

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        var JWTtoken = _userService.GenerateJwtToken(user);

        // Вернуть успешный результат с JWT
        return Ok(new { Token = JWTtoken });
        /*  // Реализуем логику проверки пользователя и пароля
        // Если пользователь найден и пароль верный, генерируется JWT токен

        var user = await _userService.AuthenticateAsync(userDto.Email, userDto.Password);

        if (user == null)
            return BadRequest(new { message = "Email or password is incorrect" });

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("здесь должен быть ваш секретный ключ");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userDto.Email),
                // Здесь вы можете добавить дополнительные claims, например, роли пользователя
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return Ok(new { Token = jwtToken });*/
    }
}