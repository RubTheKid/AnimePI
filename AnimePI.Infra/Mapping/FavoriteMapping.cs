using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimePI.Infra.Mapping;

public class FavoriteMapping : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(f => f.UserId);

        builder.Property(f => f.UserId).IsRequired();
       
        builder.Property(f => f.Animes).HasColumnType("nvarchar(max)").HasConversion(
            v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
            v => System.Text.Json.JsonSerializer.Deserialize<List<AnimeFavorite>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<AnimeFavorite>());

        builder.Property(f => f.DateCreated).IsRequired();
        builder.Property(f => f.DateUpdated);
        builder.Property(f => f.DateDeleted);
        builder.Property(f => f.IsDeleted).HasDefaultValue(false);

        builder.ToTable("Favorites");
    }
}
