using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WorkController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public WorkController(AppDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<RecentWorkComponent> workComponents = await _dbContext.RecentWorkComponents.ToListAsync();
            return View(workComponents);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RecentWorkComponent recentWorkComponent)
        {
            if (!ModelState.IsValid) return View(recentWorkComponent);

            bool isExist = await _dbContext.RecentWorkComponents
                .AnyAsync(rwc => rwc.Title.ToLower().Trim() == recentWorkComponent.Title.ToLower());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Artıq mövcuddur");
                return View(recentWorkComponent);
            }

            if (!recentWorkComponent.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Photo", "Fayl image formatında olmalıdır");
                return View(recentWorkComponent);
            }

            if (recentWorkComponent.Photo.Length / 1024 > 60)
            {
                ModelState.AddModelError("Photo", $"Şəklin ölçüsü {60} kb-dan kiçik olmalıdır");
                return View(recentWorkComponent);
            }

          

            var fileName = $"{Guid.NewGuid()}_{recentWorkComponent.Photo.FileName}";

            var path = Path.Combine(_hostEnvironment.WebRootPath, "assets/img", fileName);


            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await recentWorkComponent.Photo.CopyToAsync(fileStream);
            }

            recentWorkComponent.FilePath = fileName;

            await _dbContext.RecentWorkComponents.AddAsync(recentWorkComponent);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
    }
}
