using EFCoreMovies.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<Genre>().HasKey(p => p.Id); // for demonstration purposes only, not needed when following EF conventions
        modelBuilder.Entity<Genre>().Property(p => p.Name)
            .HasMaxLength(150)            
            .IsRequired();

        modelBuilder.Entity<Actor>().Property(p => p.Name)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<Actor>().Property(p => p.DateOfBirth)
            .HasColumnType("date");

        modelBuilder.Entity<Cinema>().Property(p => p.Name)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<CinemaHall>().Property(p => p.Cost)
            .HasPrecision(precision: 9, scale: 2);

        modelBuilder.Entity<CinemaHall>().Property(p => p.CinemaHallType)
            .HasDefaultValue(CinemaHallType.TwoDimensions);

        modelBuilder.Entity<Movie>().Property(p => p.Title)
            .HasMaxLength(250)
            .IsRequired();

        modelBuilder.Entity<Movie>().Property(p => p.ReleaseDate)
            .HasColumnType("date");

        modelBuilder.Entity<Movie>().Property(p => p.PosterURL)
            .HasMaxLength(500)
            .IsUnicode(false); // disallows use of unicode (for 'strange' characters) == saving space

        modelBuilder.Entity<CinemaOffer>().Property(p => p.DiscountPercentage)
            .HasPrecision(precision: 5, scale: 2);

        modelBuilder.Entity<CinemaOffer>().Property(p => p.Begin)
            .HasColumnType("date");

        modelBuilder.Entity<CinemaOffer>().Property(p => p.End)
            .HasColumnType("date");
    }

    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }    
    public DbSet<CinemaOffer> CinemaOffers { get; set; }    
    public DbSet<CinemaHall> CinemaHalls { get; set; }
}