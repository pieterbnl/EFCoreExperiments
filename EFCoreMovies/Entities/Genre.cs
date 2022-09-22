using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities;

[Table(name: "GenresTbl", Schema = "movies")]
public class Genre
{
    public int Id { get; set; }

    // [StringLength(maximumLength: 150)] // example of limiting property max length
    [Column("GenreName")]
    public string Name { get; set; }
}