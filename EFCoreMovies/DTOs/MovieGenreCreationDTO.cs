using EFCoreMovies.Entities;

namespace EFCoreMovies.DTOs;

public class MovieGenreCreationDTO
{
    public string Name { get; set; }
    public HashSet<Movie> Movies { get; set; }
}