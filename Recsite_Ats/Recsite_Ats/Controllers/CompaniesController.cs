using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Application.Common.Helpers;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using static Recsite_Ats.Domain.Extend.AdminSetting;

namespace Recsite_Ats.Web.Controllers;
public class CompaniesController : Controller
{
    private readonly IServices _services;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBusinessLogic _businessLogic;

    public CompaniesController(IServices services, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IBusinessLogic businessLogic)
    {
        _services = services;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _businessLogic = businessLogic;

    }
    // GET: CompaniesController
    public async Task<IActionResult> Index()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var request = new CompanyDataRequestDTO()
            {
                AccountId = user.AccountId,
                TableName = Setting.Companies.ToString(),
                Columns = Helper.GetColumnList(Setting.Companies)
            };
            var result = await _businessLogic.CompanyBusinessLogic.GetAllCompanies(request);
            ViewBag.SectionlayoutDTO = result.SectionLayout;
            return View(result.Companies);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return View();
        }
    }

    // GET: CompaniesController/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var request = new CompanyDataRequestDTO()
            {
                AccountId = user.AccountId,
                TableName = Setting.Companies.ToString(),
                Columns = Helper.GetColumnList(Setting.Companies),
                CompanyId = id
            };
            var result = await _businessLogic.CompanyBusinessLogic.GetCompany(request);
            ViewData["CompanyId"] = id;
            return View(result.CustomFields);
        }
        catch (Exception ex)
        {
            // _unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return View();
        }
    }

    // GET: CompaniesController/Create
    public async Task<ActionResult> Create()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _services.SettingService.GetFieldDetailsAsync(Setting.Companies.ToString(), user.AccountId, Helper.GetColumnList(Setting.Companies));
            result.CustomFields = result.CustomFields.Where(x => x.IsVisible).ToList();
            return View(result);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return BadRequest(ex.Message);
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCompany([FromForm] SectionLayoutDTO request)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                var fieldErrors = ModelState
                .Where(ms => ms.Key.Contains("CustomFields") && ms.Value.Errors.Count > 0)
                .Select(ms => new ErrorViewModel
                {
                    Key = ms.Key,
                    Message = ms.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                })
                .ToList();
                // Return the view with the model to show validation errors
                ViewBag.errorMessage = fieldErrors;
                return View("Create", request);
            }
            //await _businessLogic.CompanyBusinessLogic.CreateCompany(request, user.AccountId);
            return View("Create", request);
        }
        catch (Exception ex)
        {
            // _unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return View("Create", request); // Return the model to keep the form populated
        }
    }

    // POST: CompaniesController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([FromForm] SectionLayoutDTO model)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                var fieldErrors = ModelState
                .Where(ms => ms.Key.Contains("CustomFields") && ms.Value.Errors.Count > 0)
                .Select(ms => new ErrorViewModel
                {
                    Key = ms.Key,
                    Message = ms.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                })
                .ToList();
                // Return the view with the model to show validation errors
                ViewBag.errorMessage = fieldErrors;
                ViewBag.SectionlayoutDTO = model;
                return View("Index", await _unitOfWork.Company.GetAll(x => x.AccountId == user.AccountId));
            }
            //await _businessLogic.CompanyBusinessLogic.CreateCompany(model, user.AccountId);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return RedirectToAction("Index"); // Return the model to keep the form populated
        }
    }


    // GET: CompaniesController/Edit/5
    [HttpGet]
    public async Task<ActionResult> Edit(int id)
    {
        try

        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            var request = new CompanyDataRequestDTO()
            {
                AccountId = user.AccountId,
                CompanyId = id,
                TableName = Setting.Companies.ToString(),
                Columns = Helper.GetColumnList(Setting.Companies)
            };
            var result = await _businessLogic.CompanyBusinessLogic.GetCompany(request);
            ViewData["CompanyId"] = id;
            return View(result.CustomFields);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return RedirectToAction("ServerError", "Error");
        }
    }

    // POST: CompaniesController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, SectionLayoutDTO model)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var request = new CompanyDataRequestDTO()
            {
                AccountId = user.AccountId,
                CompanyId = id
            };
            // await _businessLogic.CompanyBusinessLogic.EditCompany(model.CustomFields, request);
            return RedirectToAction("Edit", id);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return View(model);
        }
    }

    // POST: CompaniesController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var request = new CompanyDataRequestDTO()
            {
                CompanyId = id,
                AccountId = user.AccountId,
                TableName = Setting.Companies.ToString(),
                Columns = Helper.GetColumnList(Setting.Companies)
            };
            await _businessLogic.CompanyBusinessLogic.DeleteCompany(request);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            ViewBag.Errors = ex.Message;
            return View();
        }
    }

}
