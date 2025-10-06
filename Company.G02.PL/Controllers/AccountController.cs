using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController( UserManager<AppUser> userManager )
        {
            _userManager = userManager;
        }
        #region SignUp
        [HttpGet]
       public IActionResult SignUp()
        {
            return View();
        }
        //P@ssW0rd
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model) 
        {
            if (ModelState.IsValid) // server-side validation
            {
               var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                  user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                         user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            //Send Email to Confirm Email   
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp !!");

            }
                return View(); 
        }
        #endregion

        #region SignIn

        #endregion

        #region SignOut

        #endregion
    }
}
