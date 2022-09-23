﻿using EFCoreMovies.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.ApplyConfiguration(new GenreConfig()); // example to apply configuration one by one
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // applies all config files in Configuration folder at once
    }

    // DbSet's to allow for querying on the tables
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }    
    public DbSet<CinemaOffer> CinemaOffers { get; set; }    
    public DbSet<CinemaHall> CinemaHalls { get; set; }    
    public DbSet<MovieActor> MoviesActors { get; set; }
}