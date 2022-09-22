using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace EFCoreMovies.Entities;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public decimal Price { get; set; }
    public Point Location { get; set; } // using NetTopologySuite.Geometries NuGet package to store coordinates (type: geography)
}