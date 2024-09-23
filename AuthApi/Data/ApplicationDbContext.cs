using AuthApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//veritabanı ile etkileşim kurar bu sayfa

namespace AuthApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DbSet'ler eklemek için burayı kullanabilirim.
        //public DbSet<LoginRequest> LoginRequests { get; set; }
        //public DbSet<RegisterRequest> RegisterRequests { get; set; }
    }
}
