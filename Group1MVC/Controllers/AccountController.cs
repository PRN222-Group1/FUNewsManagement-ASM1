using BusinessServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;
using RepositoryLayer.Enums;
using RepositoryLayer.Specifications.Account;
using System.Security.Claims;

namespace Group1MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;
        private readonly string _adminEmail;
        private readonly string _adminPassword;

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            _accountService = accountService;
            _config = config;
            _adminEmail = _config["AdminAccount:Email"];
            _adminPassword = _config["AdminAccount:Password"];
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            throw new NotImplementedException();
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            throw new NotImplementedException();
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("AccountEmail,AccountPassword")] LoginPayload loginPayload)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity identity = null;
                bool isAuthenticated = false;

                if (loginPayload.AccountEmail == _adminEmail 
                    && loginPayload.AccountPassword == _adminPassword)
                {
                    // Create the identity for the Admin
                    identity = CreateIdentity(_adminEmail, 
                        Role.Admin.ToString());

                    isAuthenticated = true;
                }
                else
                {
                    // Get the non-admin user and sign in
                    var user = await _accountService.LoginAsync(loginPayload);
                    if (user != null)
                    {
                        // Create the identity for the user
                        identity = CreateIdentity(user.AccountEmail, 
                            Enum.GetName(typeof(Role), user.AccountRole));

                        isAuthenticated = true;
                    }

                }

                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    Console.Write(User.Claims.FirstOrDefault().ToString());
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,AccountEmail,AccountRole,AccountPassword,Id")] SystemAccount systemAccount)
        {
            throw new NotImplementedException();
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            throw new NotImplementedException();
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountName,AccountEmail,AccountRole,AccountPassword,Id")] SystemAccount systemAccount)
        {
            throw new NotImplementedException();
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            throw new NotImplementedException();
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            throw new NotImplementedException();
        }

        private ClaimsIdentity CreateIdentity(string email, string role)
        {
            // Create the identity for the Admin
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            return identity;
        }
    }
}
