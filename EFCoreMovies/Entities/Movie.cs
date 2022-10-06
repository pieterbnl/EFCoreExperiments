namespace EFCoreMovies.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool InCinemas { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string PosterURL { get; set; }

    // Navigation properties
    public List<Genre> Genres { get; set; }
    public List<CinemaHall> CinemaHalls { get; set; }
    public List<MovieActor> MovieActors { get; set; }
}