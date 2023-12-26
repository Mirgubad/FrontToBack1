using FrontToBack.DAL;
using FrontToBack.ViewModels.Work;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Controllers
{
    public class WorkController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public WorkController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _appDbContext.Categories
                .Include(x => x.CategoryComponents)
                .ToListAsync();

            WorkIndexViewModel model = new WorkIndexViewModel
            {
                Categories = categories
            };
            return View(model);
        }
    }
}
