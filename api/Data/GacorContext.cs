using GacorAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GacorAPI.Data
{
    public class GacorContext : DbContext
    {
        public GacorContext(DbContextOptions<GacorContext> options) : base(options)
        {
            
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>(e => 
            {
                e.UseCollation("utf8mb4_unicode_ci");
                e.Property(p => p.Body).HasCharSet("utf8mb4");
                e.HasOne(e => e.Creator).WithMany(e => e.Blogs).HasForeignKey(e => e.CreatorId);
            });

            modelBuilder.Entity<Comment>(e =>
            {
                e.UseCollation("utf8mb4_unicode_ci");
                e.Property(p => p.Body).HasCharSet("utf8mb4");
                e.HasOne(e => e.Commenter).WithMany(e => e.Comments).HasForeignKey(e => e.CommenterId);
            });

            modelBuilder.Entity<User>(e => 
            {
                e.HasIndex(p => p.Email).IsUnique();
            });
        }
    }
}