using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMovies.Entities;

public class Actor
{
    public int Id { get; set; }
    
    // Flexible mapping
    private string _name; // field
    public string Name { 
        get { return _name; } 
        set {
            // tOm hOLLaNd => Tom Holland
            _name = string.Join(' ', 
                value.Split(' ')
                .Select(n => n[0].ToString().ToUpper() + n.Substring(1).ToLower()).ToArray());
        }
    }
    
    public string Biography { get; set; }    
    public DateTime? DateOfBirth { get; set; }

    // Navigation properties
    public HashSet<MovieActor> MovieActors { get; set; }
}