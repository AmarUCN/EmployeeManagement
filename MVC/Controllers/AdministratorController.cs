using DataAccessLayer.DAO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class AdministratorController : Controller
    {
        IAdministratorDAO _administratorDAO;

        public AdministratorController(IAdministratorDAO administratorDAO)
        {
            _administratorDAO = administratorDAO;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var administrator = _administratorDAO.Login(loginModel.Email, loginModel.Password);
                if (administrator != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, administrator.Email)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {

                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Company");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Logout() 
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Company");
        }
    }
}
