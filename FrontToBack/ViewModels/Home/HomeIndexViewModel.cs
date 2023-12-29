using FrontToBack.Models;

namespace FrontToBack.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public List<Card> Cards { get; set; }
        public List<RecentWorkComponent> RecentWorkComponents { get; set; }
        public List<Category> Categories { get; set; }
    }
}
