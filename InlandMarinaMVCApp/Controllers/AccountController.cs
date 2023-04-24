using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using InlandMarinaData;

namespace InlandMarinaMVCApp.Controllers
{
    public class AccountController : Controller
    {
        // Route: /Account/Login
        public IActionResult Login(string returnUrl = "")
        {
            if (returnUrl != null)
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        /// <summary>
        /// Authenticate user and create cookie
        /// </summary>
        /// <param name="user"></param>
        /// <returns>authenticated user or null</returns>
        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer customer) // data collected on the form
        {
            Customer cust = CustomerManager.Authenticate(customer.Username, customer.Password);
            if(customer == null) {
                return View();
            }
            //user is authenticated
            //allow user to login and view docks to create a lease
            //create a cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cust.Username),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme); // use cookies authentication
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal); // generates authentication cookie
            // if no return URL, go to the home page
            if (string.IsNullOrEmpty(TempData["ReturnUrl"].ToString()))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(TempData["ReturnUrl"].ToString());
            }
        }


        public async Task<IActionResult> LogoutAsync()
        {
            // release authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // remove  current owner from the session
            HttpContext.Session.Remove("CurrentCustomer");

            return RedirectToAction("Index", "Home"); // go to the home page
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
