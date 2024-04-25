using CaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaseProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(ApplicationDbContext db,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager,ILogger<AccountController> logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        //GET HTTP
        public async Task<IActionResult> Login()
        {
            if (!_roleManager.RoleExistsAsync(Roles.Roles.admin).GetAwaiter().GetResult())//if roles are not exist on the database, create when we click on the login page
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Roles.admin));
                await _roleManager.CreateAsync(new IdentityRole(Roles.Roles.blogger));
            }

            var adminUser = await _userManager.FindByEmailAsync("admin@example.com");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    Name = "Admin",
                    SurName = "Admin",
                };
                await _userManager.CreateAsync(adminUser, "Admin123!"); // Set the password

                // Assign the admin role to the admin user
                await _userManager.AddToRoleAsync(adminUser, "admin");
            }
            return View();
        }



        //POST HTTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                {
                    if (string.IsNullOrWhiteSpace(login.Email))
                    {
                        ModelState.AddModelError("", "Email is required");
                        _logger.LogError("Email input is empty,login failed");
                    }
                    if (string.IsNullOrWhiteSpace(login.Password))
                    {
                        ModelState.AddModelError("", "Password is required");
                        _logger.LogError("Password input is empty, login failed");
                    } 
                    return View();
                }


                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(login.Email);

                        // Check if the user is in the "Admin" role
                        var isAdmin = await _userManager.IsInRoleAsync(user, "admin");

                        if (isAdmin) //admin sayfası
                        {
                            return RedirectToAction("Register", "Account");
                        }
                        else //blogger sayfası
                        {
                            return RedirectToAction("Index", "Blog");
                        }
                }
                ModelState.AddModelError("", "Hatalı Giriş");
                _logger.LogError("Hatalı Giriş");

            }   
            return View(login);
        }

        //GET HTTP
        public async Task<IActionResult> Register()
        {
            return View();
        }


        //POST HTTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid) //server side validation
            {
                if (string.IsNullOrWhiteSpace(register.Email) || string.IsNullOrWhiteSpace(register.Password) || string.IsNullOrWhiteSpace(register.ConfirmPassword) || string.IsNullOrWhiteSpace(register.Name) || string.IsNullOrWhiteSpace(register.Surname))
                {
                    if (string.IsNullOrWhiteSpace(register.Name))
                    {
                        ModelState.AddModelError("", "Name is required");
                        _logger.LogError("Name input is empty, register failed");
                    }
                    if (string.IsNullOrWhiteSpace(register.Surname))
                    {
                        ModelState.AddModelError("", "Surname is required");
                        _logger.LogError("Surname input is empty, register failed");
                    }
                    if (string.IsNullOrWhiteSpace(register.Email))
                    {
                        ModelState.AddModelError("", "Email is required");
                        _logger.LogError("Email input is empty, register failed");
                    }
                    if (string.IsNullOrWhiteSpace(register.Password))
                    {
                        ModelState.AddModelError("", "Password is required");
                        _logger.LogError("Password input is empty, register failed");
                    }
                    if (string.IsNullOrWhiteSpace(register.ConfirmPassword))
                    {
                        ModelState.AddModelError("", "Confirm Password is required");
                        _logger.LogError("Confirm Password input is empty, register failed");
                    }
                    return View();
                }

                //create the user
                var user = new ApplicationUser { 
                    UserName = register.Email, 
                    Email = register.Email,
                    Name = register.Name,
                    SurName = register.Surname,
                };
                var result = await _userManager.CreateAsync(user,register.Password);

                    if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "blogger");
                    await _signInManager.SignInAsync(user,isPersistent:false); //sign in the new user
                    TempData["SuccessMessage"] = "Yeni kullanıcı başarıyla oluşturuldu.";
                    return RedirectToAction("Register", "Account");
                }
                //if email is duplicated
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    _logger.LogError(error.Description);
                }
            }
            return View(register);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); //log off the user
            return RedirectToAction("Index","Home");
        }
    }
}
