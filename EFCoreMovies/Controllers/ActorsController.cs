using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers;

[ApiController]
[Route("api/actors")]
public class ActorsController
{
    private readonly ApplicationDbContext _context;

    public ActorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /* Get solution with projection
     * [HttpGet]
    public async Task<IEnumerable<Actor>> Get(int page = 1, int recordsToSelect = 2)
    {
        return await _context.Actors.AsNoTracking()
            .OrderBy(a => a.Name)
            // exectute a 'projection': projecting data in a specific (actor) type, the actor type 
            .Select(a => new Actor { 
                Id = a.Id,
                Name = a.Name,
                DateOfBirth = a.DateOfBirth
                // note: purposely not including biography
            })
            .Paginate(page, recordsToSelect)
            .ToListAsync();
    }*/

    // Get solution with use of DTO
    [HttpGet]
    public async Task<IEnumerable<ActorDTO>> Get(int page = 1, int recordsToSelect = 2)
    {
        return await _context.Actors.AsNoTracking()
            .OrderBy(a => a.Name)            
            .Select(a => new ActorDTO
            {
                Id = a.Id,
                Name = a.Name,
                DateOfBirth = a.DateOfBirth                
            })
            .Paginate(page, recordsToSelect)
            .ToListAsync();
    }

    // Select a single column (results in SELECT [a].[Id]
    [HttpGet("ids")]
    public async Task<IEnumerable<int>> GetIds()
    {
        return await _context.Actors.Select(a => a.Id).ToListAsync();
    }
}