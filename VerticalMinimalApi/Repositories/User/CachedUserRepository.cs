using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace VerticalMinimalApi.Repositories.User;

public class CachedUserRepository: IUserRepository
{
    private readonly IUserRepository _decorated;
    private readonly IDistributedCache _cache;

    public CachedUserRepository(IUserRepository decorated, IDistributedCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public async Task CreateUser(Models.User user, CancellationToken ct)
    {
        await _decorated.CreateUser(user, ct);
    }

    public async Task<Models.User?> GetUserByEmail(string email, CancellationToken ct)
    {
        string key = $"user-{email}";
        string? cachedUser = await _cache.GetStringAsync(key, ct);

        Models.User? user;
        if (string.IsNullOrEmpty(cachedUser))
        {
            user = await _decorated.GetUserByEmail(email, ct);
            if (user is null) return user;
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(user), ct);
            return user;
        }

        user = JsonSerializer.Deserialize<Models.User>(cachedUser);
        return user;

    }

    public async Task<Models.User?> GetUserById(Guid id, CancellationToken ct)
    {
        string key = $"user-{id}";
        string? cachedUser = await _cache.GetStringAsync(key, ct);

        Models.User? user;
        if (string.IsNullOrEmpty(cachedUser))
        {
            user = await _decorated.GetUserById(id, ct);
            if (user is null) return user;
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(user), ct);
            return user;
        }

        user = JsonSerializer.Deserialize<Models.User>(cachedUser);
        return user;

    }
}