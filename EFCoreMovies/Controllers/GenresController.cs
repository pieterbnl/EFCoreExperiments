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
        _context.Logs.Add(new Log { Message = "Executing Get from GenresController " });
        await _context.SaveChangesAsync();
        
        return await _context.Genres.AsNoTracking()            
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    // simple but unrealistic example of modifying data using EF connected model
    [HttpPost("add2")]
    public async Task<ActionResult> Add2(int id)
    {
        // load entity
        var genre = await _context.Genres.FirstOrDefaultAsync(p => p.Id == id);

        if (genre == null)
        {
            return NotFound();
        }

        // make modification and save
        genre.Name += "2";
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> Post(GenreCreationDTO genreCreationDTO)
    {
        // var genreExists = await _context.Genres.IgnoreQueryFilters().AnyAsync(p => p.Name == genreCreationDTO.Name);
        var genreExists = await _context.Genres.AnyAsync(p => p.Name == genreCreationDTO.Name);
        
        if (genreExists)
        {
            return BadRequest($"Genre with name {genreCreationDTO.Name} already exists");
        }

        var genre = _mapper.Map<Genre>(genreCreationDTO);                
        _context.Add(genre); // marking genre as (status) added
                               
        await _context.SaveChangesAsync(); // telling EF to save all changes that are being tracked
        
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

    // Example of a 'normal' delete, removing a record from the table
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(p => p.Id == id);

        if (genre == null) return NotFound();

        _context.Remove(genre); // changes status of genre entity to 'deleted'
        await _context.SaveChangesAsync(); // will delete entity that's marked deleted

        return Ok();
    }

    // Example of a 'soft' delete, where the record is not actually deleted from the table.
    // Instead it's just marked as deleted (by a property) and no longer shown to the client.    
    [HttpDelete("softdelete/{id:int}")]
    public async Task<ActionResult> SoftDelete(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(p => p.Id == id);

        if (genre == null) return NotFound();

        genre.IsDeleted = true;
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    // Example of an endpoint to restore a soft deleted record
    [HttpPost("restore/{id:int}")]
    public async Task<ActionResult> Restore(int id)
    {
        // IgNoreQueryFilters is used to ignore the filter (in GenreConfig) that excludes soft deleted records 
        // Otherwise soft deleted records would not be loaded and thus could not be restored
        var genre = await _context.Genres.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        
        if (genre == null) return NotFound();
        
        genre.IsDeleted = false; // reversing the soft deletion of a record
        await _context.SaveChangesAsync();
        return Ok();
    }
}