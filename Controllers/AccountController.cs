using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311_PART_2.Models;
using PROG7311_PART_2.ViewModels;

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

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Role = model.Role
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign Identity role
                var roleExists = await _userManager.AddToRoleAsync(user, model.Role);

                if (!roleExists.Succeeded)
                {
                    ModelState.AddModelError("", "Role assignment failed. Make sure the role exists.");
                    return View(model);
                }

                // If Farmer, create linked profile
                if (model.Role == "Farmer")
                {
                    var farmer = new Farmer
                    {
                        Name = "Default Farmer",
                        UserId = user.Id
                    };

                    using (var scope = HttpContext.RequestServices.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        context.Farmers.Add(farmer);
                        await context.SaveChangesAsync();
                    }
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                return model.Role switch
                {
                    "Farmer" => RedirectToAction("Dashboard", "Farmer"),
                    "Employee" => RedirectToAction("Index", "Product"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            // Show Identity errors on the form
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }



    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
