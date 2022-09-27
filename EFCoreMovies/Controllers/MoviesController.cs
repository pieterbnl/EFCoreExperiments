using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Http;
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


    [HttpGet("automapper/{id:int}")]
    public async Task<ActionResult<MovieDTO>> GetWithAutoMapper(int id)
    {
        var movieDTO = await _context.Movies
            .ProjectTo<MovieDTO>(_mapper.ConfigurationProvider)            
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movieDTO == null)
        {
            return NotFound();
        }                

        movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(x => x.Id).ToList(); // filters out duplicate cinemas

        return movieDTO;
    }
}