using Escolar32.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Escolar32.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        public AccountController(UserManager<IdentityUser> userManager,
               SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);
            var user = await _userManager.FindByNameAsync(loginVM.UserName);
            
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                
                if (result.Succeeded)
                {
                    var role = await _userManager.IsInRoleAsync(user, "Admin");
                    if (role)
                    {
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }
                
                    return RedirectToAction("Aluno", "Home", new { area = "Usuario" });


                }

            }
            ModelState.AddModelError("", "Falha ao realizar Login");
            return View(loginVM);
        }

        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registroVM)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registroVM.UserName };
                var result = await _userManager.CreateAsync(user, registroVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Create", "Admin", new { area = "Admin" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("UserName", "E-mail já cadastrado. Entre com seu e-mail e senha!");
                        }
                        else
                        {

                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

            }
            
            return View(registroVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
       
    }

}