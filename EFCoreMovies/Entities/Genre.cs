using System.ComponentModel.DataAnnotations;

namespace EFCoreMovies.Entities;

public class Genre
{
    public int Id { get; set; }
    
    // [StringLength(maximumLength: 150)] // example of limiting property max length
    public string Name { get; set; }
}