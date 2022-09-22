namespace EFCoreMovies.Entities;

public class CinemaHall
{
    public int Id { get; set; }
    public decimal Cost { get; set; }
    public int CinemaId { get; set; } // FK 

    // Navigation property
    public Cinema Cinema { get; set; } 
}