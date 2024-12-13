using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using System.Diagnostics;
using ErrorViewModels = Recsite_Ats.Models.ErrorViewModel;

namespace Recsite_Ats.Controllers;

[Authorize(Roles = "Admin,Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel();
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            var claimList = User.Claims.ToList();
            model.UserName = user.UserName;
            model.Roles = roles;
            //model.Claim = claimList;
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModels { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
