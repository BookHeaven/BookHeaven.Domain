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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Tags)
            .WithMany();
        
        modelBuilder.Entity<Book>()
            .Property(b => b.Format)
            .HasDefaultValue(EbookFormat.Epub)
            .HasSentinel(EbookFormat.None);

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasDiscriminator<CollectionType>(nameof(Collection.CollectionType))
                .HasValue<SimpleCollection>(CollectionType.Simple)
                .HasValue<SmartCollection>(CollectionType.Smart);
            entity.ToTable("Collections");
        });

        // Simplified value converter setup for FilterSet<Guid> properties
        var guidFilterSetConverter = new ValueConverter<FilterSet<Guid>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<FilterSet<Guid>>(v, (JsonSerializerOptions?)null) ?? new FilterSet<Guid>()
        );
        foreach (var prop in new[] { "Authors", "Series", "Tags" })
        {
            modelBuilder.Entity<SmartCollection>()
                .Property(prop)
                .HasConversion(guidFilterSetConverter)
                .HasMaxLength(4000);
        }
        modelBuilder.Entity<SmartCollection>()
            .Property(x => x.Statuses)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<FilterSet<BookStatus>>(v, (JsonSerializerOptions?)null) ?? new FilterSet<BookStatus>()
            )
            .HasMaxLength(4000);
    }
}