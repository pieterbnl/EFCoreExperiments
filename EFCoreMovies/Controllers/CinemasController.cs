using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCoreMovies.Controllers;

[ApiController]
[Route("api/cinemas")]
public class CinemasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CinemasController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<CinemaDTO>> Get()
    {
        return await _context.Cinemas
            .ProjectTo<CinemaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }


    // Accepts input of latitude and longitude of personal current location
    // Compares distance of each cinema to personal location
    // Returns the closest cinema's (within 2km distance max)
    [HttpGet("closetome")]
    public async Task<ActionResult> Get(double latitude, double longitude)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        var myLocation = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

        var maxDistanceInMeters = 2000; // 2km's

        var cinemas = await _context.Cinemas
            .OrderBy(c => c.Location.Distance(myLocation))
            .Where(c => c.Location.IsWithinDistance(myLocation, maxDistanceInMeters))
            .Select(c => new
            {
                Name = c.Name,
                Distance = Math.Round(c.Location.Distance(myLocation))
            }).ToListAsync();
        
        return Ok(cinemas);
    }
}