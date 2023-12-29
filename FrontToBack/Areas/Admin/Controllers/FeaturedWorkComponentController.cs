using FrontToBack.Areas.Admin.ViewModels.FeaturedWorkComponent;
using FrontToBack.Areas.Admin.Views.FeaturedWorkComponent.FeaturedWorkComponentPhoto;
using FrontToBack.DAL;
using FrontToBack.Helpers;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeaturedWorkComponentController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FeaturedWorkComponentController(AppDbContext dbContext,
            IFileService fileService,
            IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var featuredWorkComponent = await _dbContext.FeaturedWorkComponent.FirstOrDefaultAsync();
            return View(featuredWorkComponent);
        }

        #region Create

        public async Task<IActionResult> Create()
        {
            var dbFeaturedWorkComponent = await _dbContext.FeaturedWorkComponent.FirstOrDefaultAsync();
            if (dbFeaturedWorkComponent != null) return NotFound();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeaturedWorkComponentCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            bool hasError = false;
            foreach (var photo in model.Photos)
            {
                if (!_fileService.IsImage(photo))
                {
                    ModelState.AddModelError("Photos", $"Fayl {photo.FileName} image olmalidir");
                    hasError = true;
                }
                int maxSize = 300;
                if (!_fileService.CheckSize(photo, maxSize))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} olcusu {maxSize} kb-dan coxdur");
                    hasError = true;
                }
            }
            if (hasError)
            {
                return View(model);
            }


            FeaturedWorkComponent featuredWorkComponent = new FeaturedWorkComponent
            {
                Description = model.Description,
                Title = model.Title,
            };

            await _dbContext.AddAsync(featuredWorkComponent);
            await _dbContext.SaveChangesAsync();


            int order = 1;
            foreach (var photo in model.Photos)
            {
                FeaturedWorkComponentPhoto featuredWorkPhoto = new()
                {
                    FeaturedWorkComponentId = featuredWorkComponent.Id,
                    Order = order++,
                    PhotoPath = await _fileService.UploadAsync(photo)
                };

                await _dbContext.AddAsync(featuredWorkPhoto);
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }


        #endregion


        #region Update

        public async Task<IActionResult> Update(int id)
        {
            var dbFeaturedWorkComponent = await _dbContext.FeaturedWorkComponent
                .Include(fp => fp.FeaturedWorkComponentPhotos)
                .FirstOrDefaultAsync();

            if (dbFeaturedWorkComponent == null) return NotFound();

            var model = new FeaturedWorkComponentUpdateVM
            {
                Title = dbFeaturedWorkComponent.Title,
                Description = dbFeaturedWorkComponent.Description,
                FeaturedWorkComponentPhotos = dbFeaturedWorkComponent.FeaturedWorkComponentPhotos
            };
            return View(model);
        }

        #endregion


        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var dbFeaturedWorkComponent = await _dbContext.FeaturedWorkComponent
                .Include(fp => fp.FeaturedWorkComponentPhotos)
                .FirstOrDefaultAsync();
            if (dbFeaturedWorkComponent == null) return NotFound();

            foreach (var photo in dbFeaturedWorkComponent.FeaturedWorkComponentPhotos)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", photo.PhotoPath);
                _fileService.Delete(path);
            }
            _dbContext.FeaturedWorkComponent.Remove(dbFeaturedWorkComponent);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        #endregion


        #region FeaturedWorkComponentPhoto

        public async Task<IActionResult> UpdatePhoto(int id)
        {
            var photo = await _dbContext.FeaturedWorkComponentPhotos.FindAsync(id);

            if (photo == null) return NotFound();
            var model = new FeaturedWorkComponentPhotoUpdateVM
            {
                Id = photo.Id,
                Order = photo.Order,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(int id, FeaturedWorkComponentPhotoUpdateVM model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid) return View(model);

            var dbPhoto = await _dbContext.FeaturedWorkComponentPhotos.FindAsync(id);
            if (dbPhoto == null) return NotFound();

            dbPhoto.Order = model.Order;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("update", "featuredworkcomponent", new { id = dbPhoto.FeaturedWorkComponentId });
        }


        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var dbPhoto = await _dbContext.FeaturedWorkComponentPhotos.FindAsync(id);
            if (dbPhoto == null) return NotFound();


            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbPhoto.PhotoPath);

            _fileService.Delete(path);

            _dbContext.FeaturedWorkComponentPhotos.Remove(dbPhoto);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("update", "featuredworkcomponent", new { id = dbPhoto.FeaturedWorkComponentId });
        }
        #endregion
    }
}
