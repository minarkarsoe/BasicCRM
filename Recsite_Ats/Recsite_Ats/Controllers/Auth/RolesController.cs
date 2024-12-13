using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Domain.Entites;

namespace Recsite_Ats.Web.Controllers.Auth;
[Authorize(Roles = "Customer")]
public class RolesController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    /*public RolesController(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    // GET: Roles
    public async Task<IActionResult> Index()
    {
        var roles = _roleManager.Roles.Select(r => new RoleViewModel
        {
            Id = r.Id,
            Name = r.Name
        }).ToList();

        return View(roles);
    }

    // GET: Roles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Roles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] RoleViewModel roleModel)
    {
        if (ModelState.IsValid)
        {
            var role = new ApplicationRole()
            {
                Name = roleModel.Name,
                RoleName = roleModel.Name
            };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(roleModel);
    }

    // GET: Roles/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var roleModel = new RoleViewModel
        {
            Id = role.Id,
            Name = role.Name
        };

        return View(roleModel);
    }

    // POST: Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] RoleViewModel roleModel)
    {
        if (id != roleModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }

            role.Name = roleModel.Name;
            role.RoleName = roleModel.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(roleModel);
    }

    // GET: Roles/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var roleModel = new RoleViewModel
        {
            Id = role.Id,
            Name = role.Name
        };

        return View(roleModel);
    }

    // POST: Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return RedirectToAction(nameof(Index));
    }*/
}
