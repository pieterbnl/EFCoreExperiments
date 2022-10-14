using EFCoreMovies.Entities;

namespace EFCoreMovies.Utilities;

public class Singleton    
{
  /*  private readonly IServiceProvider _serviceProvider;

    public Singleton(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;        
    }

    public async Task<IEnumerable<Genre>> GetGenres() 
    {
        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.Genres.ToListAsync();
        }
    }*/
}