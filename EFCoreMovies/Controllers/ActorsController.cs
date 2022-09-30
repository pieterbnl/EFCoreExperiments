using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMovies.Controllers;

[ApiController]
[Route("api/actors")]
public class ActorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ActorsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }       

    // Get solution with use of DTO & Automapper
    [HttpGet]
    public async Task<IEnumerable<ActorDTO>> Get()
    {
        return await _context.Actors.AsNoTracking()
            .OrderBy(a => a.Name)
            .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)                        
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Post(ActorCreationDTO actorCreationDTO)
    {
        var actor = _mapper.Map<Actor>(actorCreationDTO);
        _context.Add(actor);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(ActorCreationDTO actorCreationDTO, int id)
    {
        var actorDB = await _context.Actors
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (actorDB == null) return NotFound();        

        // Mapping the values from actorCreationDTO to actorDB
        // While at the same time maintaining the same instance of the actorDB object
        // This way EF knows that this instance will be updated of which it already is keeping track of in memory
        // Therefore, if in ActorCreationDTO is expressed some change, ONLY THAT property will be updated in the (SQL) query
        actorDB = _mapper.Map(actorCreationDTO, actorDB);
        await _context.SaveChangesAsync();
        return Ok();            
    }
}