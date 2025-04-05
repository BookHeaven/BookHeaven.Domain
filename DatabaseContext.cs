using Microsoft.EntityFrameworkCore;
using BookHeaven.Domain.Entities;


namespace BookHeaven.Domain;

public partial class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookProgress> BooksProgress { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProfileSettings> ProfilesSettings { get; set; }
    public DbSet<Font> Fonts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Tags)
            .WithMany();
    }
}