using AnimePI.Domain.Aggregates.AnimeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimePI.Infra.Mapping;
public class AnimeMapping : IEntityTypeConfiguration<Anime>
{
    public void Configure(EntityTypeBuilder<Anime> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.MalId).IsRequired();
        builder.HasIndex(a => a.MalId).IsUnique();

        builder.Property(a => a.Title).IsRequired().HasMaxLength(500);
        builder.Property(a => a.TitleEnglish).HasMaxLength(500);
        builder.Property(a => a.TitleJapanese).HasMaxLength(500);
        builder.Property(a => a.Synopsis).HasColumnType("nvarchar(max)");
        builder.Property(a => a.ImageUrl).HasMaxLength(1000);
        builder.Property(a => a.TrailerUrl).HasMaxLength(1000);
        builder.Property(a => a.Status).HasMaxLength(50);
        builder.Property(a => a.Type).HasMaxLength(50);

        builder.Property(a => a.Genres).HasColumnType("nvarchar(max)").HasConversion(
            v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
            v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>());


        builder.Property(a => a.DateCreated).IsRequired();
        builder.Property(a => a.DateUpdated);
        builder.Property(a => a.DateDeleted);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.IsDeleted);
        builder.HasIndex(a => a.Score);
        builder.HasIndex(a => a.Rank);

        builder.ToTable("Animes");
    }
}