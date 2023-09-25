using Carter;
using FluentValidation;
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
    public record CreateUserCommand
        (string Email, string Password, string Name, string Surname) : IRequest<Result<CreateUserResponse>>;

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();
            await _userRepository.CreateUser(user, cancellationToken);
            user = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
            return Result<CreateUserResponse>.CreateSuccess(_mapper.From(user).AddParameters("token", "waklmdsakmlfdkmsl").AdaptToType<CreateUserResponse>());
            // return BaseResponse<CreateUserResponse>.Success(_mapper.From(user)
            //     .AddParameters("token", "waklmdsakmlfdkmsl").AdaptToType<CreateUserResponse>());
        }

        public class Validation : AbstractValidator<CreateUserCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Gecersiz email adresi");
            }
        }

        public class CreateUserMapConfig : IRegister
        {
            public void Register(TypeAdapterConfig config)
            {
                config.NewConfig<User, CreateUserResponse>()
                    .Map(d => d.Token, src => MapContext.Current!.Parameters["token"]);
                config.NewConfig<CreateUserCommand, User>()
                    .Map(d => d.Id, src => Guid.NewGuid());
            }
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
            // var c = result.Response().Value.Data;
            // if (c is CreateUserResponse)
            // {   
            //     
            // }
            // Console.WriteLine(result.Response().Value);
            return result.Response();
        });
    }
}