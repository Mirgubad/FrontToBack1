using FrontToBack.Models;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Areas.Admin.ViewModels.FeaturedWorkComponent
{
    public class FeaturedWorkComponentUpdateVM
    {

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public List<IFormFile>? Photos { get; set; }

        public ICollection<FeaturedWorkComponentPhoto>? FeaturedWorkComponentPhotos { get; set; }

    }
}
