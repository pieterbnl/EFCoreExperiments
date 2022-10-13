using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities;

/*[Index(nameof(Name), IsUnique = true)]*/
public class Genre
{
    public int Id { get; set; }

    // [StringLength(maximumLength: 150)] // example of limiting property max length    
    public string Name { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public HashSet<Movie> Movies { get; set; }
}