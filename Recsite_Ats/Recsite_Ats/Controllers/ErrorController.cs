using Microsoft.AspNetCore.Mvc;

namespace Recsite_Ats.Web.Controllers;
public class ErrorController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Forbidden()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> NotFound()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> ServerError()
    {
        return View();
    }


}
