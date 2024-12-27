using CoreAPI_V3.Interface;
using CoreAPI_V3.Models;
using Microsoft.EntityFrameworkCore;
using static CoreAPI_V3.Models.ContextModel;

namespace CoreAPI_V3.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _context.Admins.SingleOrDefaultAsync(admin => admin.Username == username);
        }

        public async Task CreateAdminAsync(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
    }
}
