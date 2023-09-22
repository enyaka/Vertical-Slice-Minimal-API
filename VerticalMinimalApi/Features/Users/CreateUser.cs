using Carter;
using Mapster;
using MapsterMapper;
using MediatR;
using VerticalMinimalApi.Common.Base;
using VerticalMinimalApi.Contracts;
using VerticalMinimalApi.Models;
using VerticalMinimalApi.Repositories.User;

namespace VerticalMinimalApi.Features.Users;

public static class CreateUser
{
    public record CreateUserCommand(string Email, string Password, string Name, string Surname): IRequest<BaseResponse<CreateUserResponse>>;
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, BaseResponse<CreateUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<BaseResponse<CreateUserResponse>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();
            await _userRepository.CreateUser(user, cancellationToken);
            user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
            return BaseResponse<CreateUserResponse>.Success(_mapper.From(user).AddParameters("token", "waklmdsakmlfdkmsl").AdaptToType<CreateUserResponse>());
        }
    }
}
public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/user", async (CreateUser.CreateUserCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return result.Response();
        });
    }
}