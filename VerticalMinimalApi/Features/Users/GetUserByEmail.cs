using System.Net;
using Carter;
using MediatR;
using VerticalMinimalApi.Common.Base;
using VerticalMinimalApi.Models;
using VerticalMinimalApi.Repositories.User;

namespace VerticalMinimalApi.Features.Users;

public static class GetUserByEmail
{
    public record GetUserByEmailCommand(string Email) : IRequest<Result<User>>;

    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailCommand, Result<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<User>> Handle(GetUserByEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
            if (user is null) return Result<User>.CreateFailure("User not found", "There is no user with that email", HttpStatusCode.NotFound);
            return Result<User>.CreateSuccess(user);
        }
    }
}

public class GetUserByEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/getuserbyemail", async (GetUserByEmail.GetUserByEmailCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.Response();
        });
    }
}