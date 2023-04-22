using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MBS.Domain;
using MBS.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebMessenger.Security;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly PasswordUserSecurity _passwordUserSecurity;
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                // Здесь вы можете добавить дополнительные claims, например, роли пользователя
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public UserService(AppDbContext context, PasswordUserSecurity passwordUserSecurity)
    {
        _context = context;
        _passwordUserSecurity = passwordUserSecurity;
    }

    public async Task<User> AuthenticateAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return null;

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        // Check if the user exists and the password is correct
        if (user == null || !PasswordUserSecurity.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            return null;

        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateAsync(User user, string password)
    {
        // Validate the user data
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password is required");

        if (_context.Users.Any(u => u.Email == user.Email))
            throw new ArgumentException($"Email {user.Email} is already taken");

        // Create a salt and hash for the password
        string salt = PasswordUserSecurity.GenerateSalt();
        string passwordHash = PasswordUserSecurity.GenerateHash(password, salt);

        // Assign the password hash and salt to the user
        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;

        // Save the new user to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateAsync(User user, string password = null)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser == null)
            throw new ArgumentException("User not found");

        if (!string.IsNullOrWhiteSpace(user.Email) && user.Email != existingUser.Email)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                throw new ArgumentException($"Email {user.Email} is already taken");

            existingUser.Email = user.Email;
        }

        if (!string.IsNullOrWhiteSpace(password))
        {
            string salt = PasswordUserSecurity.GenerateSalt();
            string passwordHash = PasswordUserSecurity.GenerateHash(password, salt);

            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = salt;
        }

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}