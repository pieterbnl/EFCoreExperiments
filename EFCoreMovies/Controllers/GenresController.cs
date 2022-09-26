using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreMovies.Utilities;

namespace EFCoreMovies.Controllers;

[ApiController]
[Route("api/genres")]

public class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GenresController(ApplicationDbContext context)
    {
        _context = context;
    }

    /*[HttpGet]
    public async Task<IEnumerable<Genre>> Get()
    {
        // return await _context.Genres.AsNoTracking().ToListAsync();
        // return await _context.Genres.AsNoTracking().OrderBy(g => g.Name).ToListAsync();
        // return await _context.Genres.AsNoTracking().OrderBy(g => g.Name).Take(2).ToListAsync(); // returns the first 2 records
        return await _context.Genres.AsNoTracking().OrderBy(g => g.Name).Skip(1).Take(2).ToListAsync(); // skips first record and returns the following 2 records
    }*/

    /*    [HttpGet] // Pagination solution - 'returns an IQueryable'
        public async Task<IEnumerable<Genre>> Get(int page = 1, int recordsToSelect = 2)
        {        
            return await _context.Genres.AsNoTracking()
                .OrderBy(g => g.Name)
                .Skip((page-1)* recordsToSelect) // first page, skip 0 records
                .Take(recordsToSelect)
                .ToListAsync();
        }*/

    [HttpGet] // Pagination solution
    public async Task<IEnumerable<Genre>> Get(int page = 1, int recordsToSelect = 2)
    {
        return await _context.Genres.AsNoTracking()
            .OrderBy(g => g.Name)
            .Paginate(page, recordsToSelect)            
            .ToListAsync();
    }

    /* for demo purposes only - will return a 500 error if no records are found
    [HttpGet("first")]
    public async Task<Genre> GetFirst()
    {
        return await _context.Genres.FirstAsync(g => g.Name.Contains("p"));
    }*/

    [HttpGet("first")]
    public async Task<ActionResult<Genre>> GetFirst()
    {        
        var genre = await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Name.Contains("z"));
        
        if (genre is null)
        {
            return NotFound(); // returns a 404
        }
        return genre;
    }

    [HttpGet("filter")]
    public async Task<IEnumerable<Genre>> Filter(string name)
    {
        return await _context.Genres.Where(g => g.Name.Contains(name)).ToListAsync();
        //return await _context.Genres.Where(g => g.Name.StartsWith(name)).ToListAsync();
    }
}