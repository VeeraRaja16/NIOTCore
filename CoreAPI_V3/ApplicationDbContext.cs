using CoreAPI_V3.Models;
using Microsoft.EntityFrameworkCore;
using static CoreAPI_V3.Models.ContextModel;
using static CoreAPI_V3.Models.DashboardModel;

namespace CoreAPI_V3
{
  
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }

}

