using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recsite_Ats.Application.Common.Helpers;
using Recsite_Ats.Application.Common.Interface.Businesslogic;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using static Recsite_Ats.Domain.Extend.AdminSetting;

namespace Recsite_Ats.Web.Controllers.Settings;
[Authorize(Roles = "Customer")]
public class AdminSettingController : Controller
{
    private readonly IServices _services;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBusinessLogic _businessLogic;
    private readonly UserManager<ApplicationUser> _userManager;
    public AdminSettingController(IServices services, IUnitOfWork unitOfWork, IBusinessLogic businessLogic, UserManager<ApplicationUser> userManager)
    {
        _services = services;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _businessLogic = businessLogic;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            return View();
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CompanySetting()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _businessLogic.AdminSettingBusinessLogic.GetSetting(Setting.Companies.ToString(), user.AccountId, Helper.GetColumnList(Setting.Companies));
            return View(result);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> JobSetting()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _businessLogic.AdminSettingBusinessLogic.GetSetting(Setting.Jobs.ToString(), user.AccountId, Helper.GetColumnList(Setting.Jobs));
            return View(result);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ContactSetting()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _businessLogic.AdminSettingBusinessLogic.GetSetting(Setting.Contacts.ToString(), user.AccountId, Helper.GetColumnList(Setting.Contacts));
            return View(result);
        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> CandidateSetting()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            var result = await _businessLogic.AdminSettingBusinessLogic.GetSetting(Setting.Candidates.ToString(), user.AccountId, Helper.GetColumnList(Setting.Candidates));
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
    public async Task<IActionResult> UpdateOrder([FromBody] List<CustomFieldViewModel> order)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                //_unitOfWork.Logging(this, error.ErrorMessage);
            }
            return StatusCode(500, errors);
        }

        if (order == null || !order.Any())
        {
            return StatusCode(400, "Order data is missing or invalid.");
        }

        try
        {
            var updateFieldLayouts = new List<FieldLayout>();
            var insertFieldLayouts = new List<FieldLayout>();
            foreach (var item in order)
            {
                FieldLayout fieldLayout;

                if (item.Id.HasValue && item.Id.Value != 0)
                {
                    fieldLayout = await _unitOfWork.FieldLayout.Get(f => f.Id == item.Id);

                    if (fieldLayout == null)
                    {
                        return StatusCode(404, $"FieldLayout with ID {item.Id} not found.");
                    }

                    updateFieldLayouts.Add(PopulateFieldLayout(fieldLayout, item));
                }
                else
                {
                    // Create a new field layout for insertion
                    fieldLayout = PopulateFieldLayout(new FieldLayout(), item);
                    insertFieldLayouts.Add(fieldLayout);
                }

            }
            if (insertFieldLayouts.Any())
            {
                await _unitOfWork.FieldLayout.BulkAdd(insertFieldLayouts);
                await _unitOfWork.Save();
            }
            // Bulk update existing field layouts
            if (updateFieldLayouts.Any())
            {
                await _unitOfWork.FieldLayout.BulkUpdate(updateFieldLayouts);
                await _unitOfWork.Save();
            }
            return Json(new { success = true });

        }
        catch (Exception ex)
        {
            //_unitOfWork.Logging(this, ex.Message);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCustomSection([FromBody] SectionLayoutViewModel sectionLayout)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var section = await _unitOfWork.Section.Get(s => s.SectionName == sectionLayout.SectionName && s.TableName == sectionLayout.TableName && (s.AccountId == user.AccountId || s.AccountId == null));
                if (section != null)
                {
                    return Json(new { success = false, error = "Section is already existed." });
                }

                // Fetch the maximum SortOrder for the account
                int maxSortOrder = await _unitOfWork.Section.Max(s => s.Sort, s => (s.AccountId == user.AccountId || s.AccountId == null) && s.IsCustomSection && s.TableName == sectionLayout.TableName);

                var newSection = new SectionLayout()
                {
                    SectionName = sectionLayout.SectionName,
                    TableName = sectionLayout.TableName,
                    AccountId = user.AccountId,
                    IsCustomSection = true,
                    Visible = true,
                    Sort = maxSortOrder + 1 // Increment the max sort order for the new section
                };

                await _unitOfWork.Section.Add(newSection);
                await _unitOfWork.Save();
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                //_unitOfWork.Logging(this, ex.Message);
                return StatusCode(500, new { success = false, error = ex.Message });
            }

        }

        var errors = ModelState.ToDictionary(
            k => k.Key,
            k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
        //_unitOfWork.Logging(this, JsonSerializer.Serialize(errors));
        return Json(new { success = false, errors });
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCustomSection(int sectionId, string sectionName)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var section = await _unitOfWork.Section.Get(s => s.SectionLayoutId == sectionId);
                if (section == null)
                {
                    return Json(new { success = false, error = "Section does not exist." });
                }

                section.SectionName = sectionName;
                await _unitOfWork.Section.Update(section);
                await _unitOfWork.Save();
                return Json(new { success = true, message = "Update Section Name Successfully!" });
            }
            catch (Exception ex)
            {
                //_unitOfWork.Logging(this, ex.Message);
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        var errors = ModelState.ToDictionary(
          k => k.Key,
          k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()
          );
        //_unitOfWork.Logging(this, JsonSerializer.Serialize(errors));
        return Json(new { success = false, errors });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCustomSection(int sectionId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var section = await _unitOfWork.Section.Get(s => s.SectionLayoutId == sectionId && s.AccountId == user.AccountId);
                if (section == null)
                {
                    return Json(new { success = false, error = "Section is not found." });
                }
                var fieldLayoutsBySections = await _unitOfWork.FieldLayout.GetAll(f => f.SectionLayoutId == sectionId && f.AccountId == user.AccountId);
                if (fieldLayoutsBySections == null || fieldLayoutsBySections.Count() == 0)
                {
                    _unitOfWork.Section.Remove(section);
                    await _unitOfWork.Save();
                    return Json(new { success = true, message = "Successfully Deleted!" });
                }
                var defaultSection = await _unitOfWork.Section.Get(s => !s.IsCustomSection && s.TableName == section.TableName);
                int getMaxSortOrder = await _unitOfWork.FieldLayout.Max(f => (int)f.Sort, f => f.SectionLayoutId == defaultSection.SectionLayoutId && f.AccountId == user.AccountId);
                foreach (var fieldLayout in fieldLayoutsBySections)
                {
                    fieldLayout.SectionLayoutId = defaultSection.SectionLayoutId;
                    fieldLayout.Sort = getMaxSortOrder + 1;
                    await _unitOfWork.FieldLayout.Update(fieldLayout);
                    getMaxSortOrder++;
                }
                _unitOfWork.Section.Remove(section);

                var sectionLayouts = await _unitOfWork.Section.GetAll(f => f.TableName == section.TableName && f.AccountId == user.AccountId && f.IsCustomSection && f.SectionName != section.SectionName);
                int index = 1;
                foreach (var sectionLayout in sectionLayouts.OrderBy(s => s.Sort))
                {
                    sectionLayout.Sort = index;
                    await _unitOfWork.Section.Update(sectionLayout);
                    index++;
                }
                await _unitOfWork.Save();
                return Json(new { success = true, message = "Successfully Deleted!" });
            }
            catch (Exception ex)
            {
                //_unitOfWork.Logging(this, ex.Message);
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
        var errors = ModelState.ToDictionary(
           k => k.Key,
           k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()
       );
        return Json(new { success = false, errors });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCustomField(CreateCustomFieldViewModel request)
    {
        var redirect = TempData["RedirectUrl"].ToString().Split('/');
        string tableNameStr = TempData["TableName"].ToString();
        Enum.TryParse<Setting>(tableNameStr, out Setting tableName);

        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var fieldTypes = await _unitOfWork.FieldType.Get(x => x.Id == request.FieldTypeId);
                if (fieldTypes.FieldTypeName.ToLower() == "MultiSelect".ToLower())
                {
                    if (string.IsNullOrEmpty(request.ViewValue))
                    {
                        ModelState.AddModelError("ViewValue", "Comma seperated value is required.");
                        var data = await _businessLogic.AdminSettingBusinessLogic.GetSetting(tableName.ToString(), user.AccountId, Helper.GetColumnList(tableName));
                        return View(redirect[2], data);
                    }
                }
                var defaultSection = await _unitOfWork.Section.Get(s => !s.IsCustomSection && s.TableName == request.TableName);
                int getMaxSortOrder = await _unitOfWork.FieldLayout.Max(f => (int)f.Sort, f => f.SectionLayoutId == defaultSection.SectionLayoutId && f.AccountId == user.AccountId);
                var fieldLayOut = new FieldLayout()
                {
                    AccountId = user.AccountId.Value,
                    TableName = request.TableName,
                    FieldName = request.FieldName,
                    SectionLayoutId = defaultSection.SectionLayoutId,
                    Sort = getMaxSortOrder == 0 ? 9999 : getMaxSortOrder + 1,
                    Required = false,
                    Visible = true,
                    IsCustomField = true,
                };
                await _unitOfWork.FieldLayout.Add(fieldLayOut);
                var customField = new CustomField()
                {
                    AccountId = user.AccountId.Value,
                    TableName = request.TableName,
                    FieldName = request.FieldName,
                    FieldAlias = "_" + request.FieldName.ToLower(),
                    FieldTypeId = request.FieldTypeId,
                    ViewValues = request.ViewValue,
                };
                await _unitOfWork.CustomField.Add(customField);
                await _unitOfWork.Save();
                return RedirectToAction(redirect[2], redirect[1]);
            }
            catch (Exception ex)
            {
                //_unitOfWork.Logging(this, ex.Message);
                return RedirectToAction(redirect[2], redirect[1]);
            }
        }
        else
        {
            return RedirectToAction(redirect[2], redirect[1]);
        }
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCustomField([FromBody] EditCustomerFieldViewModel request)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var customField = await _unitOfWork.CustomField.Get(c => c.Id == request.CustomFieldId && c.AccountId == user.AccountId);
                if (customField == null)
                {
                    return Json(new { success = false, error = "Custom Field is not found." });
                }
                var fieldLayout = await _unitOfWork.FieldLayout.Get(f => f.FieldName == customField.FieldName && f.TableName == customField.TableName && f.AccountId == user.AccountId);
                customField.FieldName = request.FieldName;
                customField.ViewValues = request.ViewValue;
                customField.FieldAlias = "_" + request.FieldName.ToLower();
                if (fieldLayout != null)
                {
                    fieldLayout.FieldName = customField.FieldName;
                    await _unitOfWork.FieldLayout.Update(fieldLayout);
                }
                await _unitOfWork.CustomField.Update(customField);
                await _unitOfWork.Save();
                return Json(new
                {
                    success = true,
                    message = "Successfully Updated!",
                    data = new
                    {
                        updatedFieldName = customField.FieldName,
                        updatedViewValue = customField.ViewValues
                    }
                });
            }
            catch (Exception ex)
            {
                //_unitOfWork.Logging(this, ex.Message);
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
        var errors = ModelState.ToDictionary(
           k => k.Key,
           k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()
       );
        return Json(new { success = false, errors });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCustomFields(int fieldId)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var customField = await _unitOfWork.CustomField.Get(c => c.Id == fieldId && c.AccountId == user.AccountId);
                if (customField == null)
                {
                    return Json(new { success = false, error = "Custom Field is not found." });
                }
                var fieldLayoutByCustomField = await _unitOfWork.FieldLayout.Get(f => f.TableName == customField.TableName && f.AccountId == user.AccountId && f.FieldName == customField.FieldName);
                if (fieldLayoutByCustomField != null)
                {
                    _unitOfWork.FieldLayout.Remove(fieldLayoutByCustomField);
                }
                _unitOfWork.CustomField.Remove(customField);
                await _unitOfWork.Save();
                return Json(new { success = true, message = "Successfully Deleted!" });
            }
            catch (Exception ex)
            {
                //_unitOfWork.Logging(this, ex.Message);
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }
        var errors = ModelState.ToDictionary(
           k => k.Key,
           k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()
       );
        return Json(new { success = false, errors });
    }

    private FieldLayout PopulateFieldLayout(FieldLayout fieldLayout, CustomFieldViewModel item)
    {
        fieldLayout.SectionLayoutId = item.SectionLayoutId;
        fieldLayout.AccountId = item.AccountId;
        fieldLayout.FieldName = item.FieldName;
        fieldLayout.TableName = item.TableName;
        fieldLayout.Required = item.IsRequired;
        fieldLayout.Visible = item.IsVisible;
        fieldLayout.IsCustomField = item.IsCustomField;
        fieldLayout.Sort = item.SortOrder;
        return fieldLayout;
    }
}


