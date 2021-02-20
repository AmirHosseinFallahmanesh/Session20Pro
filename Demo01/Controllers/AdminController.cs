using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo01.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo01.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordValidator<AppUser> passwordValidator;
        private readonly IUserValidator<AppUser> userValidator;
        private readonly IPasswordHasher<AppUser> passwordHasher;

        public AdminController(UserManager<AppUser> userManager,
            IPasswordValidator<AppUser> passwordValidator,
            IUserValidator<AppUser> userValidator,
            IPasswordHasher<AppUser> passwordHasher)
        {
            this.userManager = userManager;
            this.passwordValidator = passwordValidator;
            this.userValidator = userValidator;
            this.passwordHasher = passwordHasher;
        }
        public IActionResult Index()
        {
          var users=  userManager.Users.ToList();

            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach (var item in users)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    Id=item.Id,
                    Email = item.Email,
                    Password = item.PasswordHash,
                    Username = item.UserName
                };
                userViewModels.Add(userViewModel);
               
            }

            return View(userViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = model.Username,
                    Email = model.Email

                };

               IdentityResult result= await userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }


            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string Id)
        {
              AppUser user= await userManager.FindByIdAsync(Id);
            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,

                Username = user.UserName
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {

           AppUser user= await userManager.FindByIdAsync(model.Id);
            if (user==null)
            {

                ModelState.AddModelError("", "User Not Found");
                return View(model);
            }
            if (ModelState.IsValid)
            {

                user.UserName = model.Username;
                user.Email = model.Email;
               //var modelValidatorResult= await userValidator.ValidateAsync(userManager, user);

               // if (!modelValidatorResult.Succeeded)
               // {
               //     AddModelError(modelValidatorResult);
                 
               // }

                var passwordValidateResult = await passwordValidator.ValidateAsync(userManager, user, model.Password);
                if (!passwordValidateResult.Succeeded)
                {
                    AddModelError(passwordValidateResult);

                }
                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);


                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddModelError(result);
                }

            }
            return View(model);
        }

        public async Task< IActionResult> Delete(string Id)
        {
            AppUser user = await userManager.FindByIdAsync(Id);
            if (user!=null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                return RedirectToAction("index");
            }
            return View();
        }
        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }


    }
}