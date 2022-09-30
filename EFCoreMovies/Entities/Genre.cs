using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities;

public class Genre
{
    public int Id { get; set; }

    // [StringLength(maximumLength: 150)] // example of limiting property max length    
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public HashSet<Movie> Movies { get; set; }
}