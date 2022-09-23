using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Configurations;

public class CinemaOfferConfig : IEntityTypeConfiguration<CinemaOffer>
{
    public void Configure(EntityTypeBuilder<CinemaOffer> builder)
    {
        builder.Property(p => p.DiscountPercentage).HasPrecision(precision: 5, scale: 2);
    }
}