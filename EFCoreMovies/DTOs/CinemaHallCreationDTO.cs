using EFCoreMovies.Entities;

namespace EFCoreMovies.DTOs;

public class CinemaHallCreationDTO
{
    public int Id { get; set; } // required for updating/PUT endpoint action. Note: could also create separate Update DTO with id
    public double Cost { get; set; }
    public CinemaHallType CinemaHallType { get; set; }
}