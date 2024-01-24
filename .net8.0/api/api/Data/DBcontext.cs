using api.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
#pragma warning disable CS1591
    public class DBcontext : IdentityDbContext
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> Users {  get; set; }
    }
#pragma warning restore CS1591
}
