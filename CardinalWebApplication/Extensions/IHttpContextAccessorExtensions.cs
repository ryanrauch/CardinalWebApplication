using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace CardinalWebApplication.Extensions
{
    public static class IHttpContextAccessorExtensions
    {
        public static string CurrentUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            return stringId ?? String.Empty;
        }
    }

}
