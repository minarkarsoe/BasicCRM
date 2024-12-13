using Recsite_Ats.Domain.DataTransferObject.UserDTO;

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface IAdminService
{
    Task RegisterRecruiterAsync(RegisterRecruiterDto model, string adminId);
}
