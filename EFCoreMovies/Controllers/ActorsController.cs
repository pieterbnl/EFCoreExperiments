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
public class ActorsController
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
}