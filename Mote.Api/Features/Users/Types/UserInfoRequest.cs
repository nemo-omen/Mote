namespace Mote.Api.Features.Users.Types;

public record UserInfoRequest
{
    public required string Email { get; init; }
}