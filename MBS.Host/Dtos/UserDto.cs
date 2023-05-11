using MBS.Domain.Entities;

namespace MBS.Host.Dtos;

public class UserDto
{
    public string Username { get; set; }

    public UserStatus Status { get; set; }

    public string AboutMe { get; set; }
}