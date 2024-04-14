using NutriPro.Application.Configurations.Filters;
using NutriPro.Application.Interfaces.Management;
using NutriPro.Data.Models.Management;
using NutriPro.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NutriPro.Mvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly NutriProSession _session;
        private readonly IUsersAppService _userAppService;
        private readonly IAuthService _authService;

        public AuthController(NutriProSession session, IUsersAppService userAppService, IAuthService authService)
        {
            _session = session;
            _userAppService = userAppService;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View(new LoginVM());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var loggedInUser = await _userAppService.ValidateUserCredentials(model.Login, model.Password);

            if (loggedInUser != null)
            {
                var token = _authService.GenerateJwtToken(model.Login);
                var redirectTo = Url.Action("Index", "Home");

                if (loggedInUser.FullName == "Admin Master")
                {
                    _session.SetHostMode();
                }
                else
                {
                    _session.SetUserAndTenant(
                        loggedInUser.Id,
                        loggedInUser.TenantId,
                        loggedInUser.FullName,
                        loggedInUser.Units.Select(x => x.Id).ToList()
                    );
                }

                var response = new { token, redirectTo };
                return Ok(response);
            }

            return BadRequest(new { errorMessage = "Credenciais inválidas." });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _session.Clear();
            var response = new
            {
                redirectTo = Url.Action("Index", "Auth")
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("RenewToken")]
        public IActionResult RenewToken(string tokenFixed)
        {
            if (tokenFixed == "8ee6fc18-ff62-408d-b785-c8d73efbab00")
            {
                var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest("Usuário não autenticado");
                }

                var token = _authService.GenerateJwtToken(userEmail);

                return Ok(new { token });
            }

            return BadRequest();
        }
    }
}
