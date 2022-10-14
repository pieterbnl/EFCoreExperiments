using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities;

public class Actor
{
    public int Id { get; set; }   
    public string Name { get; set; }
    public string Biography { get; set; }    
    public DateTime? DateOfBirth { get; set; }    

    // Navigation properties
    public HashSet<MovieActor> MovieActors { get; set; }
    public Address Addresses { get; set; }
}