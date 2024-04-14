using NutriPro.Application.Configurations.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace NutriPro.Mvc
{
    public class JwtAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private readonly IAuthService _authService;

        public JwtAuthorizationFilter(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.ToString().StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader.ToString().Substring("Bearer ".Length).Trim();
            var isValidToken = _authService.ValidateToken(token);

            if (!isValidToken)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
