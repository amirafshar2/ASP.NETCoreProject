using BE.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Concrete
{
    /// <summary>EF-Core-Datenbankkontext (SQLite) inkl. ASP.NET Core Identity.</summary>
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogRating> BlogRatings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Writer> Writers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender).WithMany(w => w.SentMessages)
                .HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver).WithMany(w => w.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Writer>()
                .HasOne(w => w.AppUser).WithOne(u => u.Writer)
                .HasForeignKey<Writer>(w => w.AppUserId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Rating).WithOne(r => r.Blog)
                .HasForeignKey<BlogRating>(r => r.BlogId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category).WithMany(c => c.Blogs)
                .HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Writer).WithMany(w => w.Blogs)
                .HasForeignKey(b => b.WriterId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Blog).WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Blog>().Property(b => b.Title).HasMaxLength(150).IsRequired();
            modelBuilder.Entity<Blog>().Ignore(b => b.ReadingMinutes);
            modelBuilder.Entity<BlogRating>().Ignore(r => r.Average);
            modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(60).IsRequired();
            modelBuilder.Entity<NewsLetter>().HasIndex(n => n.Mail).IsUnique();
        }
    }
}
