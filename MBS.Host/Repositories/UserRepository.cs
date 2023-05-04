using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Host.InfrastructureServices;
using Microsoft.EntityFrameworkCore;

namespace MBS.Host.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
}