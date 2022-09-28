using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public MoviesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Eager loading example (loading data from related record via navigation properties)
    // + Ordering and filtering with DTO 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDTO>> Get(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Genres // using navigation property to load related Genres data
                .OrderByDescending(g => g.Name) // set order
                .Where(g => !g.Name.Contains("m")))  // filter on a criteria
            .Include(m => m.CinemaHalls
                .OrderByDescending(ch => ch.Cinema.Name))
                .ThenInclude(ch => ch.Cinema)
            .Include(m => m.MoviesActors)
                .ThenInclude(ma => ma.Actor)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        var movieDTO = _mapper.Map<MovieDTO>(movie); // projection

        movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList(); // filters out duplicate cinemas

        return movieDTO;
    }

    // Ordering and filtering via automapper example
    [HttpGet("automapper/{id:int}")]
    public async Task<ActionResult<MovieDTO>> GetWithAutoMapper(int id)
    {
        var movieDTO = await _context.Movies
            .ProjectTo<MovieDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movieDTO == null) return NotFound();

        movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList(); // filters out duplicate cinemas

        return movieDTO;
    }

    // Select loading example
    [HttpGet("selectLoading/{id:int}")]
    public async Task<ActionResult> GetSelectLoading(int id)
    {
        var movieDTO = await _context.Movies
            .Select(m => new // just new, anonymous type
            { // load Id, Title and Genres
                Id = m.Id,
                Title = m.Title,
                Genres = m.Genres
                        .Select(g => g.Name) // free to do a select over the related data
                        .OrderByDescending(n => n).ToList()
            })
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movieDTO == null) return NotFound();

        return Ok(movieDTO);
    }

    // Explicit loading example
    [HttpGet("explicitLoading/{id:int}")]
    public async Task<ActionResult<MovieDTO>> GetExplicit(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

        // load related data, now main data is loaded
        await _context
            .Entry(movie)
            .Collection(p => p.Genres)
            .LoadAsync();

        if (movie == null) return NotFound();

        var genresCount = await _context
            .Entry(movie)
            .Collection(prop => prop.Genres)
            .Query()
            .CountAsync();

        var movieDTO = _mapper.Map<MovieDTO>(movie);

        return Ok(new
        {
            Id = movieDTO.Id,
            Title = movieDTO.Title,
            GenresCount = genresCount
        });
    }

    // Lazy loading example
    [HttpGet("lazyLoading/{id:int}")]
    public async Task<ActionResult<MovieDTO>> GetLazyLoading(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        var movieDTO = _mapper.Map<MovieDTO>(movie); // projection

        movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList(); // filters out duplicate cinemas

        return movieDTO;
    }

    // Grouping by using the GroupBy function
    // Grouping by InCinemas property in 2 categories:
    // 1) where InCinemas is true, and where InCinemas is false
    [HttpGet("groupedByCinema")]
    public async Task<ActionResult> GetGroupByCinema()
    {
        var groupedMovies = await _context.Movies.GroupBy(m => m.InCinemas)
            .Select(g => new
            {
                InCinemas = g.Key, // Key is equal to whatever value in m.InCinemas
                Count = g.Count(), // Count operation over group
                Movies = g.ToList() // List of movies in the group
            }).ToListAsync();

        return Ok(groupedMovies);
    }

    [HttpGet("groupByGenresCount")]
    public async Task<ActionResult> GetGroupedByGenresCount()
    {
        var groupedMovies = await _context.Movies.GroupBy(m => m.Genres.Count()).Select(g => new 
        {
            Count = g.Key,
            Titles = g.Select(m => m.Title),
            Genres = g.Select(m => m.Genres)
                .SelectMany(a => a) // Using SelectMany to flatten a collection of a collection.. into a single collection; a of array
                    .Select(ge => ge.Name).Distinct() // Collection of genres
       }).ToListAsync();

        return Ok(groupedMovies);
    }

    // Filter movies by title, genre, and if they are in cinema
    // Dynamic in the sense that, it's only applied if the user wants to
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> Filter([FromQuery] MovieFilterDTO movieFilterDTO)
    { // FromQuery because MovieFilterDTO is complext type, and using HttpGet.FromQuery means values from MovieFilterDTO comes from query strings.
        var moviesQueryable = _context.Movies.AsQueryable();

        if (!string.IsNullOrEmpty(movieFilterDTO.Title))
        { // If Title is not null, put in Queryable a Where clause/filter
            moviesQueryable = moviesQueryable.Where(m => m.Title.Contains(movieFilterDTO.Title));
        }

        if (movieFilterDTO.InCinemas)
        {
            moviesQueryable = moviesQueryable.Where(m => m.InCinemas);
        }

        if (movieFilterDTO.UpcomingReleases)
        {
            var today = DateTime.Today;
            moviesQueryable = moviesQueryable.Where(m => m.ReleaseDate > today);
        }

        if (movieFilterDTO.GenreId != 0)
        {
            moviesQueryable = moviesQueryable
           .Where(m => m.Genres.Select(g => g.Id).Contains(movieFilterDTO.GenreId));
        }

        var movies = await moviesQueryable.Include(m => m.Genres).ToListAsync();
        
        return _mapper.Map<List<MovieDTO>>(movies);
    }
}