using FrontToBack.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<ContactIntro> ContactIntro { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryComponent> CategoryComponents { get; set; }
        public DbSet<RecentWorkComponent> RecentWorkComponents { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<FeaturedWorkComponent> FeaturedWorkComponent { get; set; }
        public DbSet<FeaturedWorkComponentPhoto> FeaturedWorkComponentPhotos { get; set; }





    }
}
