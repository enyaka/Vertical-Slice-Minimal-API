namespace VerticalMinimalApi.Repositories.User;

public interface IUserRepository
{
    Task CreateUser(Models.User user, CancellationToken ct);
    Task<Models.User?> GetUserByEmail(string email, CancellationToken ct);
    Task<Models.User?> GetUserById(Guid id, CancellationToken ct);
}