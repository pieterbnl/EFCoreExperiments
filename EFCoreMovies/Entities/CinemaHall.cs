namespace EFCoreMovies.Entities;

public class CinemaHall
{
    public int Id { get; set; }
    public CinemaHallType CinemaHallType { get; set; }
    public decimal Cost { get; set; }
    public int CinemaId { get; set; } // FK 

    // Navigation properties
    public Cinema Cinema { get; set; }
    public HashSet<Movie> Movies { get; set; }
}