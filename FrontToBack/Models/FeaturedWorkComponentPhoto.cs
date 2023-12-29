namespace FrontToBack.Models
{
    public class FeaturedWorkComponentPhoto
    {
        public int Id { get; set; }
        public string PhotoPath { get; set; }
        public int FeaturedWorkComponentId { get; set; }
        public int Order { get; set; }
        public FeaturedWorkComponent FeaturedWorkComponent { get; set; }

    }
}
