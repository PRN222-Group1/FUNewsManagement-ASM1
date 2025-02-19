using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Group1MVC.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(AccountSpecParams specParams)
        {
            var accounts = await _accountService.GetAccountsAsync(specParams);

            var count = await _accountService.CountAccountsAsync(specParams);

            var paginatedResult = new Pagination<SystemAccountDTO>(specParams.PageNumber, specParams.PageSize, count, accounts);

            // Set current filter and sort values for use in the view
            ViewData["CurrentFilter"] = specParams.Search;
            ViewData["CurrentSort"] = specParams.Sort;
            ViewData["CurrentPageNumber"] = specParams.PageNumber;
            ViewData["CurrentPageSize"] = specParams.PageSize;
            ViewData["CurrentPageCount"] = Convert.ToInt32(Math.Ceiling((decimal)count / specParams.PageSize));

            // Selected role
            ViewData["SelectedRole"] = specParams.Role;

            return View(paginatedResult);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var account = await _accountService.GetAccountByIdAsync(id.Value);

            if (account == null) return NotFound();

            return View(account);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            var loginPayload = new LoginPayload() { 
                AccountEmail = "",
                AccountPassword = ""
            };
            return View(loginPayload);
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
                        Role.Admin.ToString(), -1);

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
                            user.AccountRole, user.Id);

                        isAuthenticated = true;
                    }
                }

                // If is authenticated create a claims principal and sign in with that claims
                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorMessage"] = "Incorrect email or password or the user account is deleted!";
            return View();
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            ViewData["Action"] = "Create";

            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountName,AccountEmail,AccountRole,AccountPassword,AccountConfirmPassword")] SystemAccountToAddOrUpdateDTO systemAccount)
        {
            var result = false;

            if (!ModelState.IsValid)
            {
                // Return the view with the invalid model and validation errors
                ViewData["Action"] = "Create";
                return View(systemAccount);
            }

            result = await _accountService.CreateAccountAsync(systemAccount);

            if (result)
            {
                TempData["SuccessMessage"] = "Create Account Successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error creating account!";
            return View(systemAccount);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _accountService.GetAccountByIdAsync(id.Value);

            if (account == null)
            {
                return NotFound();
            }

            ViewData["Action"] = "Edit";

            // Convert account role from string to enum value
            var role = (int)Enum.Parse(typeof(Role), account.AccountRole);
            ViewBag.SelectedRole = role;

            var accountToUpdate = new SystemAccountToAddOrUpdateDTO
            {
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = role
            };

            return View(accountToUpdate);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountName,AccountEmail,AccountRole,AccountPassword,AccountConfirmPassword")] SystemAccountToAddOrUpdateDTO systemAccount)
        {
            if (systemAccount.AccountPassword != systemAccount.AccountConfirmPassword)
            {
                TempData["ErrorMessage"] = "Confirm password is not the same as password!";

                // Re-populate ViewData and return the view with the model
                ViewData["Action"] = "Edit";
                ViewBag.SelectedRole = systemAccount.AccountRole;
                return View(systemAccount);
            }

            if (!ModelState.IsValid)
            {
                // If validation fails, return view with validation errors
                ViewData["Action"] = "Edit";
                ViewBag.SelectedRole = systemAccount.AccountRole;
                return View(systemAccount);
            }

            var result = await _accountService.UpdateAccountAsync(id, systemAccount);

            if (result)
            {
                TempData["SuccessMessage"] = "Update Account Successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error updating account!";
            return View(systemAccount);
        }


        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _accountService.DeleteAccountAsync(id.Value);

            if (result)
            {
                TempData["SuccessMessage"] = "Delete account successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Delete account failed!";
            }

            return RedirectToAction("Index");
        }

        private ClaimsIdentity CreateIdentity(string email, string role, int? id)
        {
            // Create the identity for the Admin
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, id.Value.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            return identity;
        }
    }
}
