namespace Recsite_Ats.Domain.ViewModels;


public class AdminSettingViewModel
{
    public List<SettingViewModel> Settings { get; set; }
}

public class SettingViewModel
{
    public string SettingName { get; set; }
    public string Link { get; set; }
}
