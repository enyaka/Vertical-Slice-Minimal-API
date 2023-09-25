using Microsoft.EntityFrameworkCore;
using VerticalMinimalApi.Context;

namespace VerticalMinimalApi.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly MinimalDbContext _context;

    public UserRepository(MinimalDbContext context)
    {
        _context = context;
    }

    public async Task CreateUser(Models.User user, CancellationToken ct)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Models.User?> GetUserByEmail(string email, CancellationToken ct)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<Models.User?> GetUserById(Guid id, CancellationToken ct)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
    }
}