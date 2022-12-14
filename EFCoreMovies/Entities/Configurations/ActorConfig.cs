using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Configurations;

public class ActorConfig : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Biography).HasColumnType("nvarchar(max)");


        // builder.Ignore(p => p.Age); // example of ignoring a property, to prevent it from being mapped and thus not being stored in the database

        // builder.Property(p => p.Name).HasField("_name"); // not needed, because we're following naming convention already
    }
}