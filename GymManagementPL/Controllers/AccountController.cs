using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager)
        {
            this.accountService = accountService;
            this.signInManager = signInManager;
        }

        #region Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var User = accountService.ValidateUser(model);

            if(User is null )
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Password or Email");
                return View(model);
            }
            
            var Result = signInManager.PasswordSignInAsync(User , model.Password , model.RememberMe, false).Result;

            if(Result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account Is not Allowed");

            if(Result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account Is Locked Out");

            if(Result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(model);
        }




        #endregion

        #region Logout

        public ActionResult Logout()
        {

            signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));



        }


        #endregion


        public ActionResult AccessDenied()
        {
            return View();
        }












    }
}
