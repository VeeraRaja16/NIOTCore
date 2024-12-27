using CoreAPI_V3.Models;
//using static CoreAPI_V3.Models.ContextModel;

namespace CoreAPI_V3.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
