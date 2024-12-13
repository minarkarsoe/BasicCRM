using Microsoft.AspNetCore.Identity;

namespace Recsite_Ats.Domain.Entites;
public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
}
