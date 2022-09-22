using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Entities;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; }    
    public decimal Price { get; set; }
    public string Location { get; set; }
}