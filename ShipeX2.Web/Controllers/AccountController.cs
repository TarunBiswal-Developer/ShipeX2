using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShipeX2.Application.Interfaces;
using System.Security.Claims;

namespace ShipeX2.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAuthenticationService _authService;

        public AccountController ( IUserAuthenticationService authService )
        {
            _authService = authService;
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Login ()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var name = User.FindFirst(ClaimTypes.Name)?.Value;
                var redirectRoute = _authService.GetRedirectRouteByRole(role);
                HttpContext.Session.SetString("UserRole", role);
                HttpContext.Session.SetString("UserName", name);
                return RedirectToAction(redirectRoute.Action, redirectRoute.Controller);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login ( string username, string password )
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                        ViewBag.Error = "Username and Password are required.";
                    else if (string.IsNullOrEmpty(username))
                        ViewBag.Error = "Username is required.";
                    else
                        ViewBag.Error = "Password is required.";

                    return View();
                }


                var result = await _authService.AuthenticateUserAsync(username, password);

                if (!result.IsSuccess)
                {
                    ViewBag.Error = result.ErrorMessage;
                    return View();
                }

                // Sign in
                await HttpContext.SignInAsync("auth_token", result.Principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(30)
                });

                // Set session values
                var role = result.Principal.FindFirst(ClaimTypes.Role)?.Value;
                var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
                HttpContext.Session.SetString("UserRole", role);
                HttpContext.Session.SetString("UserName", name);

                var redirectRoute = _authService.GetRedirectRouteByRole(role);
                return RedirectToAction(redirectRoute.Action, redirectRoute.Controller);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unexpected error occurred.";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout ()
        {
            await HttpContext.SignOutAsync("auth_token");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied () => View("AccessDenied");
    }
}
