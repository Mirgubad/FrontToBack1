using FrontToBack.Areas.Admin.ViewModels.CategoryComponent;
using FrontToBack.DAL;
using FrontToBack.Helpers;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryComponentController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryComponentController(AppDbContext dbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var categoryComponents = await _dbContext.CategoryComponents
                .Include(ct => ct.Category)
                .OrderByDescending(ct => ct.Id)
                .ToListAsync();
            return View(categoryComponents);
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            var model = new CategoryComponentCreateVM
            {
                Categories = await _dbContext.Categories
                .Select(ct => new SelectListItem()
                {
                    Text = ct.Title,
                    Value = ct.Id.ToString(),
                }).ToListAsync()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryComponentCreateVM model)
        {
            model.Categories = await _dbContext.Categories
                .Select(ct => new SelectListItem()
                {
                    Text = ct.Title,
                    Value = ct.Id.ToString()
                }).ToListAsync();

            if (!ModelState.IsValid) return View(model);

            bool isExist = await _dbContext.CategoryComponents
                    .AnyAsync(cc => cc.Title.ToLower().Trim() == model.Title.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Artiq movcuddur");
                return View(model);
            }

            if (!_fileService.IsImage(model.Photo))
            {
                ModelState.AddModelError("Photo", "Fayl image formatinda olmalidir");
                return View(model);
            }

            int maxSize = 60;
            if (!_fileService.CheckSize(model.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"Şəklin ölçüsü {maxSize} kb-dan boyukdur");
                return View(model);
            }

            var category = await _dbContext.Categories.FindAsync(model.CategoryId);

            if (category == null) return NotFound();


            var categoryComponent = new CategoryComponent()
            {
                FilePath = await _fileService.UploadAsync(model.Photo),
                Title = model.Title,
                Description = model.Description,
                CategoryId = model.CategoryId,
            };

            await _dbContext.CategoryComponents.AddAsync(categoryComponent);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }


        #endregion


        #region Update

        public async Task<IActionResult> Update(int id)
        {
            CategoryComponent categoryComponent = await _dbContext.CategoryComponents.FindAsync(id);
            if (categoryComponent == null) return NotFound();

            var model = new CategoryComponentUpdateVM
            {
                Id = categoryComponent.Id,
                Title = categoryComponent.Title,
                Description = categoryComponent.Description,
                FilePath = categoryComponent.FilePath,
                CategoryId = categoryComponent.CategoryId,
                Categories = await _dbContext.Categories.Select(ct => new SelectListItem()
                {
                    Value = ct.Id.ToString(),
                    Text = ct.Title
                }).ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryComponentUpdateVM model)
        {
            if (id != model.Id) return BadRequest();

            model.Categories = await _dbContext.Categories.Select(ct => new SelectListItem
            {
                Value = ct.Id.ToString(),
                Text = ct.Title
            }).ToListAsync();

            if (!ModelState.IsValid) return View(model);

            bool isExist = await _dbContext.CategoryComponents
                .AnyAsync(cc => cc.Title.ToLower().Trim() == model.Title.ToLower().Trim() && cc.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Artiq movcuddur");
                return View(model);
            }
            var dbCategoryComponent = await _dbContext.CategoryComponents.FindAsync(id);
            if (dbCategoryComponent == null) return NotFound();

            dbCategoryComponent.Title = model.Title;
            dbCategoryComponent.Description = model.Description;
            dbCategoryComponent.CategoryId = model.CategoryId;


            if (model.Photo != null)
            {
                if (!_fileService.IsImage(model.Photo))
                {
                    ModelState.AddModelError("Photo", "Fayl image olmalidir");
                    return View(model);
                }
                int maxSize = 60;
                if (!_fileService.CheckSize(model.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"Şəkilin ölçüsü {maxSize} kb-dan çoxdur");
                    return View(model);
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbCategoryComponent.FilePath);
                _fileService.Delete(path);
                dbCategoryComponent.FilePath = await _fileService.UploadAsync(model.Photo);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
    }
}
