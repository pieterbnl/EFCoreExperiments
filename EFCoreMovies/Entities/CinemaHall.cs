namespace EFCoreMovies.Entities;

public class CinemaHall
{
    public int Id { get; set; }
    public CinemaHallType CinemaHallType { get; set; }
    public decimal Cost { get; set; }
    public int CinemaId { get; set; } // FK 

    // Navigation property
    public virtual Cinema Cinema { get; set; }
    public virtual HashSet<Movie> Movies { get; set; }
}