using MediatR;

namespace VerticalMinimalApi.Features.Users;

public static class CreateUser
{
    public record CreateUserRecord(string Email, string Password, string Name);
}