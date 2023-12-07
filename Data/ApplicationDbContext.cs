using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebPulse_WebManager.Models;

namespace WebPulse_WebManager.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WebPulse_WebManager.Models.Group> Group { get; set; } = default!;
        public DbSet<WebPulse_WebManager.Models.Website> Website { get; set; } = default!;
        public DbSet<WebPulse_WebManager.Models.Credential> Credential { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        
    }
}