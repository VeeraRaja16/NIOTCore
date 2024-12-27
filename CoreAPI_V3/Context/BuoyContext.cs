using Microsoft.EntityFrameworkCore;

namespace CoreAPI_V3.Context
{
    public class BuoyContext : DbContext
    {
        private readonly string _connectionString;

        public BuoyContext()
        {
        }

        // Constructor accepting IConfiguration to retrieve the connection string
        public BuoyContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // Override OnConfiguring to apply the connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString); // Configure SQL Server as the database provider
                                                                //  optionsBuilder.CommandTimeout(380); // Optional: Set command timeout to 380 seconds
            }
        }
    }
}
