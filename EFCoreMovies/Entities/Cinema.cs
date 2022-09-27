using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace EFCoreMovies.Entities;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; }

    // using NetTopologySuite.Geometries NuGet package to store coordinates (type: geography)
    // be aware this cannot be serialized
    public Point Location { get; set; } 

    // navigation properties
    public virtual CinemaOffer CinemaOffer { get; set; } // because only one offer per cinema, this is a 1:1 relationship
    public virtual HashSet<CinemaHall> CinemaHalls { get; set; } // note that HashSet has a better performance than ICollection, but does not guarantee an ordered result
}