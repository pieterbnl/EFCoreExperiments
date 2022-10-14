using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreMovies.DTOs;
using EFCoreMovies.Entities;
using EFCoreMovies.Entities.Keyless;
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
    private readonly IServiceProvider _context;
    private readonly IMapper _mapper;

    public CinemasController(IServiceProvider context, IMapper mapper)
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

    [HttpGet("withoutlocation")]
    public async Task<IEnumerable<CinemaWithoutLocation>> GetCinemaWithoutLocation()
    {
        /*return await _context.Set<CinemaWithoutLocation>().ToListAsync(); // note: Set creates a dbset on the fly*/
        return await _context.CinemaWithoutLocations.ToListAsync(); // note: Set creates a dbset on the fly
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
                    Currency = Currency.Peso,
                    CinemaHallType = CinemaHallType.TwoDimensions
                },
                new CinemaHall()
                {
                    Cost = 250,
                    Currency = Currency.USDollar,
                    CinemaHallType = CinemaHallType.ThreeDimensions
                },
            }
        };

        _context.Add(cinema);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("withDTO")]
    public async Task<ActionResult> CreateCinema(CinemaCreationDTO cinemaCreationDTO)
    {
        var cinema = _mapper.Map<Cinema>(cinemaCreationDTO);
        _context.Add(cinema);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Example of only updating related data (of cinema) directly,
    // by using the corresponding db set
    [HttpPut("cinemaOffer")]
    public async Task<ActionResult> PutCinemaOffer(CinemaOffer cinemaOffer)
    {
        _context.Update(cinemaOffer);
        await _context.SaveChangesAsync();
        return Ok();        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(int id)
    {
        var cinemaDB = await _context.Cinemas
           .Include(c => c.CinemaHalls)
           .Include(c => c.CinemaOffer)
           .FirstOrDefaultAsync(c => c.Id == id);

        if (cinemaDB == null) return NotFound();

        cinemaDB.Location = null;
        
        return Ok(cinemaDB);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(CinemaCreationDTO cinemaCreationDTO, int id)
    {
        // Load cinema from database
        // Include ensures that the related entities are updated
        // I.e.: this will update the cinema, cinemahalls and cinemaoffer
        var cinemaDB = await _context.Cinemas
            .Include(c => c.CinemaHalls)
            .Include(c => c.CinemaOffer)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cinemaDB == null) return NotFound();

        // This method will be able to handle all usecases:
        // not only for updating the principal entity, but also
        // for updating (or deleting) the related entities
        cinemaDB = _mapper.Map(cinemaCreationDTO, cinemaDB);
        await _context.SaveChangesAsync();
        return Ok();       
    }
}