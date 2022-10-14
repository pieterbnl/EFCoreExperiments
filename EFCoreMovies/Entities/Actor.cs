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
    
    [NotMapped]
    public int? Age { get
        {
            if (!DateOfBirth.HasValue)
            {
                return null;
            }
            else
            {
                // calculating age, based on date of birth                
                var dateOfBirth = DateOfBirth.Value;
                var age = DateTime.Today.Year - dateOfBirth.Year;

                // checking if birthday has already occured or not
                if (new DateTime(DateTime.Today.Year, dateOfBirth.Month, dateOfBirth.Day) > DateTime.Today)
                {
                    age--;
                }

                return age;
            }                       
        }
    }

    public string PictureURL { get; set; }

    // Navigation properties
    public HashSet<MovieActor> MovieActors { get; set; }
    public Address Addresses { get; set; }
}