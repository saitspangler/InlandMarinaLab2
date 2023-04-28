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
        public async Task<IActionResult> Login(Customer customer) // data collected on the form
        {
            Customer cust = CustomerManager.Authenticate(customer.Username, customer.Password);
            if(cust == null) {
                return View();
            }
            //user is logged in and authenticated with cookie authentication
            //create and stores cookie in the browser for the user
            //cookie is encrypted and sent to the browser
            else
            {
                //create claims
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, cust.Username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, cust.ID.ToString()));
                //create identity
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //create principal
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                //sign in
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //add current customer to the session
                HttpContext.Session.SetString("CurrentCustomer", cust.ID.ToString());
                Console.WriteLine(cust);
                //redirect to home page
                return RedirectToAction("Index", "Lease");
            }
        }


        public async Task<IActionResult> Logout()
        {
            // release authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // remove  current owner from the session
            HttpContext.Session.Remove("CurrentCustomer");

            return RedirectToAction("Index", "Home"); // go to the home page
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            //add a new customer to the database from form data collected on the page
            try
            {
                CustomerManager.AddCustomer(customer);
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
