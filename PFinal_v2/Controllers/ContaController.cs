using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PFinal_v2.Models; 


public class ContaController : Controller
{
    private readonly LoginService _loginService;

    public ContaController(LoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _loginService.FindByEmailAndPasswordAsync(username, password);
        if (user != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Nome),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Colaborador"),
            new Claim("UsuarioId", user.UsuarioId.ToString())
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Dias");
            }
        }

        ViewBag.ErrorMessage = "Usuário ou senha inválidos";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }


    public IActionResult RedirectUser()
    {
        if (User.Identity.IsAuthenticated)
        {
            if (User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Dias");
            }
        }
        else
        {
            return RedirectToAction("Login");
        }
    }
}
