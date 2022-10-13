using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Entities.Keyless;
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

   /* // Ordering and filtering via automapper example
    [HttpGet("automapper/{id:int}")]
    public async Task<ActionResult<MovieDTO>> GetWithAutoMapper(int id)
    {
        var movieDTO = await _context.Movies
            .ProjectTo<MovieDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movieDTO == null) return NotFound();

        movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList(); // filters out duplicate cinemas

        return movieDTO;
    }*/

    [HttpGet("withCounts")]
    public async Task<ActionResult<IEnumerable<MoviesWithCounts>>> GetMoviesWithCounts()
    {
        return await _context.Set<MoviesWithCounts>().ToListAsync();
    }

    [HttpGet("getmovie/{id:int}")]
    public async Task<ActionResult<Movie>> Get(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Genres) // using navigation property to load related Genres data
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();
        
        return movie;
    }

    [HttpPost]
    public async Task<ActionResult> Post(MovieCreationDTO movieCreationDTO)
    {
        var movie = _mapper.Map<Movie>(movieCreationDTO);

        // Only want to create a relationship between the movie and the (by the client provided) related Genres and Cinemahalls
        // Not actually create new genres and cinemahalls.
        // Thus, will change the EF status to prevent tracking/updating of those entities.
        movie.Genres.ForEach(g => _context.Entry(g).State = EntityState.Unchanged);
        movie.CinemaHalls.ForEach(ch => _context.Entry(ch).State = EntityState.Unchanged);

        if (movie.MovieActors != null)
        {
            for (int i = 0; i <movie.MovieActors.Count; i++)
            {
                movie.MovieActors[i].Order = i + 1;
            }
        }
        
        _context.Add(movie);        
        await _context.SaveChangesAsync();        
        return Ok();
    }
}