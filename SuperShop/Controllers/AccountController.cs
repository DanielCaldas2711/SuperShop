﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty,"Failed to login");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Customer");

                    var isInRole = await _userHelper.IsUserInRoleAsync(user, "Customer");

                    if (!isInRole)
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Customer");
                    }

                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RemenberMe = false,
                        UserName = model.Username
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged.");
                    

                }
            }
            return View(model);
        }

        public async Task<ActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,response.Errors.FirstOrDefault().Description);
                    }
                }
            }            

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null) 
                {

                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty,result.Errors.FirstOrDefault().Description);
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }                
            }
            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
