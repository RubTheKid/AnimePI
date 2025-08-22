using AnimePI.Domain.Aggregates.FavoriteAggregate;
using AnimePI.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimePI.Infra.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.OwnsOne(c => c.Email, eb =>
        {
            eb.Property(c => c.Mail).IsRequired().HasColumnName("Email").HasColumnType("varchar(255)");
        });

        builder.OwnsOne(u => u.UserName, eb =>
        {
            eb.Property(x => x.FirstName).IsRequired().HasColumnName("Name").HasColumnType("varchar(64)");
            eb.Property(x => x.Surname).IsRequired().HasColumnName("Surname").HasColumnType("varchar(64)");
            eb.Ignore(x => x.FullName);
        });

        builder.Property(u => u.DateCreated).IsRequired();
        builder.Property(u => u.DateUpdated);
        builder.Property(u => u.DateDeleted);
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);

        builder.HasOne(u => u.Favorites)
            .WithOne()
            .HasForeignKey<Favorite>("UserId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("IsDeleted");

        builder.ToTable("Users");
    }
}
