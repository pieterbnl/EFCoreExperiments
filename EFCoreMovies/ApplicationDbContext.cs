using EFCoreMovies.Entities;
using EFCoreMovies.Entities.Keyless;
using EFCoreMovies.Entities.Seeding;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCoreMovies;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // new conventions
        // if a property is of datetime, type should be date (and not datetime2)
        configurationBuilder.Properties<DateTime>().HaveColumnType("date");

        // make every string by default maxlength 150
        configurationBuilder.Properties<string>().HaveMaxLength(150);
    }

    // OnModelCreating is used to configure the properties of the entities and relationships between them, using the FluentApi
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.ApplyConfiguration(new GenreConfig()); // example to apply configuration one by one
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // applies all config files in Configuration folder at once
        Seeding.Seed(modelBuilder);

        // passing arbitrary query to Sql - no database is created for this cinema with a location entity
        modelBuilder.Entity<CinemaWithoutLocation>().ToSqlQuery("Select Id, Name FROM Cinemas").ToView(null);

        modelBuilder.Entity<MoviesWithCounts>().ToView("MoviesWithCounts");

        // modelBuilder.Entity<Log>().Property(p => p.Id).ValueGeneratedNever(); // for example only
        // modelBuilder.Ignore<Address>(); // example on how to prevent EF from mapping a class, and thus not saving in Database

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) // provides access to each entity
        {
            // iterate their properties
            foreach (var property in entityType.GetProperties())
            {
                // see what property of data type is, access the name and check if it contains 'url'
                if (property.ClrType == typeof(string) 
                    && property.Name.Contains("URL", StringComparison.CurrentCultureIgnoreCase))
                {
                    property.SetIsUnicode(false); 
                }
                
            }
        }

    }

    // DbSet's to allow for querying on the tables
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }    
    public DbSet<CinemaOffer> CinemaOffers { get; set; }    
    public DbSet<CinemaHall> CinemaHalls { get; set; }    
    public DbSet<MovieActor> MoviesActors { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<CinemaWithoutLocation> CinemaWithoutLocations { get; set; }
}