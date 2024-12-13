using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Recsite_Ats.Domain.ViewModels;
public class SectionLayoutViewModel
{
    public string TableName { get; set;  }
    public string SectionName { get; set;}
    public int AccountId { get; set; }
    public int Sort { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsCustomSection { get; set; } = true;
}
