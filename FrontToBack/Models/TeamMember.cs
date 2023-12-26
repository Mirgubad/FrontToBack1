using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Position { get; set; }
        public string? PhotoName { get; set; }
        [Required]
        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
