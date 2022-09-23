using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Configurations;

public class MovieActorConfig : IEntityTypeConfiguration<MovieActor>
{
    public void Configure(EntityTypeBuilder<MovieActor> builder)
    {

        // Indicate composed key for MovieActor, consisting of MovieId and ActorId
        // Note: order of Id's doesn't matter
        builder.HasKey(p => new { p.MovieId, p.ActorId });
    }
}