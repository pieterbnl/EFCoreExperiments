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

    [HttpPost]
    public async Task<ActionResult> Post()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var cinemaLocation = geometryFactory.CreatePoint(new Coordinate(-69.913539, 18.476256));

        // Create cinema object and populate it's properties,
        // including related CinemaOffer and CinemaHalls
        
        var cinema = new Cinema() 
        {
            Name = "My cinema",
            Location = cinemaLocation,
            CinemaOffer = new CinemaOffer()
            {
                DiscountPercentage = 5,
                Begin = DateTime.Today,
                End = DateTime.Today.AddDays(7)
            },
            CinemaHalls = new HashSet<CinemaHall>()
            {
                new CinemaHall()
                {
                    Cost = 200,
                    CinemaHallType = CinemaHallType.TwoDimensions
                },
                new CinemaHall()
                {
                    Cost = 250,
                    CinemaHallType = CinemaHallType.ThreeDimensions
                },
            }
        };

        _context.Add(cinema);
        await _context.SaveChangesAsync();
        return Ok();
    }
}