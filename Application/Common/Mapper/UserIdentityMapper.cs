using Application.UserIdentity.Commands;
using Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace Application.Common.Mapper;

[Mapper]
public static partial class UserIdentityMapper
{
    public static partial User ToUser(this RegisterCommand command);
}