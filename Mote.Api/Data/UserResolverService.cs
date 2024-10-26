using System.Security.Claims;
using System.Security.Principal;
using FluentResults;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mote.Api.Models;
// using Mote.Api.Shared.Utils;
using Microsoft.AspNetCore.Identity;

namespace Mote.Api.Data;

public interface IUserResolverService
{
    string GetUserIdentityName();
    string? GetUserId();
    IIdentity? GetUserIdentity();
}

public class UserResolverService : IUserResolverService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserResolverService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserIdentityName()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var userIdName = string.Empty;
        
        if (httpContext is null)
        {
            return userIdName;
        }

        var sessionIdentity = httpContext.User.Identity;
        if (sessionIdentity is null || !sessionIdentity.IsAuthenticated)
        {
            return userIdName;
        }

        if (sessionIdentity.Name != null)
        {
            userIdName = sessionIdentity.Name;
        }
        return userIdName;
    }
    
    public IIdentity? GetUserIdentity()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.User.Identity;
    }

    public string? GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}