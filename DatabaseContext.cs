using Microsoft.EntityFrameworkCore;
using BookHeaven.Domain.Entities;
using BookHeaven.Domain.Entities.Base;
using BookHeaven.Domain.Enums;
using System.Text.Json;
using BookHeaven.Domain.Entities.Utilities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


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
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<KoreaderProgress> KoreaderProgress { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}