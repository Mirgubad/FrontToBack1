using FrontToBack.Models;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Areas.Admin.ViewModels.FeaturedWorkComponent
{
    public class FeaturedWorkComponentCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }


    }
}
