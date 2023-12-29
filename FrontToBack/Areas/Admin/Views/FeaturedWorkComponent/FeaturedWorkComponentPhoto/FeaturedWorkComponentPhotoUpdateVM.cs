using Microsoft.Build.Framework;

namespace FrontToBack.Areas.Admin.Views.FeaturedWorkComponent.FeaturedWorkComponentPhoto
{
    public class FeaturedWorkComponentPhotoUpdateVM
    {
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
