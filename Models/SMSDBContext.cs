using Microsoft.EntityFrameworkCore;

namespace SMS_MVC.Models
{
    public class SMSDBContext : DbContext
    {
        public SMSDBContext(DbContextOptions<SMSDBContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
