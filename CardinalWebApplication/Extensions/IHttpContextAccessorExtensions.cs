using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CardinalWebApplication.Extensions
{
    public static class IHttpContextAccessorExtensions
    {
        public static string CurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return stringId ?? String.Empty;
        }
    }

}
