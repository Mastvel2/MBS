using System;
using System.Linq;
using System.Threading.Tasks;
using MBS.Domain;
using MBS.Domain.Services;
using MBS.Host.Dtos;
using MBS.Host.Host_services;
using WebMessenger.Security;

public class Identity
{
    private readonly AppDbContext _dbContext;
    private readonly IUserService _userService;

    public Identity(AppDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }
    
    /// <summary>
    /// Регистрирует нового пользователя с указанным адресом
    /// электронной почты и паролем. Генерирует соль и хеш пароля, используя класс
    /// PasswordUserSecurity. Возвращает false,
    /// если пользователь с таким адресом электронной почты уже существует,
    /// и true в случае успешной регистрации.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<AuthenticationResult> Register(string email, string password)
    {
        try
        {
            var user = new User { Email = email };
            var createdUser = await _userService.CreateAsync(user, password);

            if (createdUser != null)
            {
                return new AuthenticationResult
                {
                    IsSuccess = true,
                    Token = null // Токен не нужен при регистрации
                };
            }
            else
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Не удалось зарегистрировать пользователя"
                };
            }
        }
        catch (ArgumentException ex)
        {
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Аутентифицирует пользователя с указанным адресом электронной почты и паролем.
    /// Возвращает объект User, если аутентификация прошла успешно, и null в противном случае.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public User Authenticate(string email, string password)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

        if (user == null)
        {
            return null;
        }

        bool isPasswordValid = PasswordUserSecurity.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

        if (!isPasswordValid)
        {
            return null;
        }

        return user;
    }
}