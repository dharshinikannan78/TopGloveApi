using Microsoft.EntityFrameworkCore;
using topGlove.Model;

namespace topGlove.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
        {

        }
        public DbSet<UserDetails> Login { get; set; }
        
        public DbSet<TrayTrackinInput> TrayDetails { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetails>().ToTable("userdetails");
            modelBuilder.Entity<TrayTrackinInput>().ToTable("traytrackingdetails");
        }
        }
}
