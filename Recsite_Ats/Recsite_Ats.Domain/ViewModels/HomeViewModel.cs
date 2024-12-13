using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Recsite_Ats.Domain.ViewModels;
public class HomeViewModel
{
    public string UserName { get; set; }
    public IList<string> Roles { get; set; }
    public List<Claim> Claim { get; set; }
}
