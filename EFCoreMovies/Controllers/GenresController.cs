using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreMovies.Utilities;
using EFCoreMovies.DTOs;
using AutoMapper;

namespace EFCoreMovies.Controllers;

[ApiController]
[Route("api/genres")]

public class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GenresController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    

    [HttpGet] 
    public async Task<IEnumerable<Genre>> Get()
    {
        return await _context.Genres.AsNoTracking()
            .OrderBy(g => g.Name)            
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Post(GenreCreationDTO genreCreationDTO)
    {
        var genre = _mapper.Map<Genre>(genreCreationDTO);
        // var status1 = _context.Entry(genre).State;
        
        _context.Add(genre); // marking genre as (status) added
        // _context.Genres.Add(genre); // same - but specifying explicitly about Genres; not required
        
        // var status2 = _context.Entry(genre).State;
                
        await _context.SaveChangesAsync(); // telling EF to save all changes that are being tracked
        
        // var status3 = _context.Entry(genre).State;

        return Ok();
    }

    [HttpPost("several")]
    public async Task<ActionResult> Post(GenreCreationDTO[] genresDTO)
    {
        var genres = _mapper.Map<Genre[]>(genresDTO);
        /* for demo purposes only; not a good solution to add multiple records
        foreach (var genre in genres)
        {
            _context.Add(genre);
        } */
        _context.AddRange(genres);
        
        await _context.SaveChangesAsync(); // EF will iterate through all entities its tracking, and apply the corresponding operations according to the status of each entity
        
        return Ok();
    }    
}