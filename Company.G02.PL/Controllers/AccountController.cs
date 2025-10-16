using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController( UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid) // server-side validation
            {
               var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                  var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                      var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe , false);
                        //SignIn
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");

                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Login !!");
            }
            return View(model);
        }

        #endregion

        #region SignOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync(); // بيحذف الكوكيز الخاصة بتسجيل الدخول
            return RedirectToAction("SignIn", "Account"); // بيرجع المستخدم لصفحة تسجيل الدخول
        }

        #endregion

        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {    
                var user =  await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    // Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //Create URL to reset password
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email , token },Request.Scheme);
                    // Create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };
                    //send email to reset password  
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {

                        // Check your inbox
                        return RedirectToAction("CheckYourInbox");

                    }
                }
            }
            ModelState.AddModelError("", "Invalid Email !!");
            return View("ForgetPassword",model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email , string token)
        {
            TempData["email"] = email;  
            TempData["token"] = token;
            return View();
        }
        //P@ssW0rrd
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email == null || token == null)
                {
                    return BadRequest("Invalid Operation");
                }

                if (email is not null && token is not null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        
                    }
                    ModelState.AddModelError("", "Invalid Rest Password Operation !!");

                }
            }
            return View();
        }
        #endregion

    }
}
