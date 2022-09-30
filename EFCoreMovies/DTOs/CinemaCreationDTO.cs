namespace EFCoreMovies.DTOs;

public class CinemaCreationDTO
{
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public CinemaOfferCreationDTO CinemaOffer { get; set; } // represents related data of cinema
    public CinemaHallCreationDTO[] CinemaHalls { get; set; } // represents related data of cinema
}