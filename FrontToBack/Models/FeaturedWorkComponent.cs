﻿namespace FrontToBack.Models
{
    public class FeaturedWorkComponent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<FeaturedWorkComponentPhoto> FeaturedWorkComponentPhotos { get; set; }

    }
}
