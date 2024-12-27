using CoreAPI_V3.Models;
using static CoreAPI_V3.Models.ContextModel;

namespace CoreAPI_V3.Interface
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task CreateAdminAsync(Admin admin);
        Task UpdateAdminAsync(Admin admin);
    }
}
