using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Recsite_Ats.Domain.ViewModels.ComponentViewModel;

namespace Recsite_Ats.Web.Components;

public class ListViewComponents : ViewComponent
{
    public IViewComponentResult Invoke(string someParameter)
    {
        var model = new CompanyViewComponentModel
        {
            Data = GetData(someParameter)
        };
        return View(model);
    }

    private JArray GetData(string parameter)
    {
        var jsonArray = new JArray();
        // Logic to get data
        var json = new JObject {
            { "Name" , parameter}
        };
        jsonArray.Add(json);
        return jsonArray;
    }
}
