using Microsoft.EntityFrameworkCore;
using MvcFilm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MvcFilm.Entities
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
      
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Notification> Notifications { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Movie>()
               .ToTable(tb => tb.HasTrigger("trg_AddMovieNotification"));
        }
    }


}
