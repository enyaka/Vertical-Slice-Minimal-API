using Mapster;
using VerticalMinimalApi.Contracts;
using VerticalMinimalApi.Features.Users;
using VerticalMinimalApi.Features.Users.Common;
using VerticalMinimalApi.Models;

namespace VerticalMinimalApi.Common.Mapper;

public class UserMapConfig: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, CreateUserResponse>()
            .Map(d => d.Token, src => MapContext.Current!.Parameters["token"]);
        config.NewConfig<CreateUser.CreateUserCommand, User>()
            .Map(d => d.Id, src => Guid.NewGuid());
    }
}