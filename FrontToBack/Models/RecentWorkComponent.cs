using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Models
{
    public class RecentWorkComponent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? FilePath { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
