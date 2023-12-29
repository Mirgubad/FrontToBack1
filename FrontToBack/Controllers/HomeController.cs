using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var cards = await _appDbContext.Cards.ToListAsync();
            var recentWorkComponents = await _appDbContext.RecentWorkComponents.ToListAsync();
            var categories = await _appDbContext.Categories
                .Include(cc => cc.CategoryComponents)
                .ToListAsync();

            HomeIndexViewModel model = new HomeIndexViewModel
            {
                Categories = categories,
                RecentWorkComponents = recentWorkComponents
            };

            return View(model);
        }
    }
}
