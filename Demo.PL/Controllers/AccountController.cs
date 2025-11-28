using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register
        //Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Check server-side validation
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // You can also sign in the user here if you want
                    return RedirectToAction("Login");
                }

                // If Identity returns errors, add them to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we reach here → Model invalid OR user creation failed
            return View(model);
        }


        #endregion

        #region Login
        //Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)// server-side validation
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {
                        // Sign in the user
                        // You can use SignInManager here if you have it injected
                        var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                        if (loginResult.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                }
                ModelState.AddModelError(string.Empty, "Email doesn't exists.");
            }
            return View(model);
        }

        #endregion

        #region SignOut

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Forget Password

        // Shows the "Forgot Password" page
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgotPasswordViewModel model)
        {
            // Ensure the form inputs are valid
            if (ModelState.IsValid)
            {
                // Check if the email exists in the database
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    // Step 1: Generate a unique password-reset token for this user
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Step 2: Encode token to make it safe inside a URL 
                    // (tokens may contain + or / which break URLs)
                    var encodedToken = WebEncoders.Base64UrlEncode(
                        Encoding.UTF8.GetBytes(token)
                    );

                    // Step 3: Create a full reset-password link that will be emailed to the user
                    var resetPasswordLink = Url.Action("ResetPassword", "Account",
                        new { email = user.Email, token = encodedToken }, Request.Scheme);

                    // Step 4: Prepare the email body and send it
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = resetPasswordLink
                    };

                    EmailSettings.SendEmail(email);

                    // Step 5: Redirect user to "Check your inbox" page
                    return RedirectToAction("CheckYourInbox");
                }
                else
                {
                    // User does not exist in the system
                    ModelState.AddModelError(string.Empty, "Email does not exist");
                }
            }

            // Return same page with validation errors
            return View("ForgotPassword", model);
        }

        #endregion

        #region CheckYourInbox

        // Simply shows a page telling user to check their email
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password

        // This opens the Reset Password page when user clicks the email link
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            // Validate: URL must contain email & token
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                // If link is altered or broken, redirect back
                return RedirectToAction("ForgotPassword");
            }

            // Pass the data to the reset-password page
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // If user did not fill form correctly
            if (!ModelState.IsValid)
                return View(model);

            // Step 1: Check that this email belongs to a real user
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid request");
                return View(model);
            }

            // Step 2: Token must be decoded because it was encoded earlier
            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(model.Token)
            );

            // Step 3: Try resetting the password using Identity
            var result = await _userManager.ResetPasswordAsync(
                user, decodedToken, model.NewPassword
            );

            // Step 4: If password changed successfully, show success page
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            // Step 5: Otherwise, show the errors (invalid token, weak password, etc.)
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        #endregion

        // Final page after password has been successfully reset
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }



    }
}
