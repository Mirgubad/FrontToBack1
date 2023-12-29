using FrontToBack.Areas.Admin.ViewModels.Team;
using FrontToBack.DAL;
using FrontToBack.Helpers;
using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public TeamController(AppDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService

            )
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<TeamMember> teamMembers = await _dbContext.TeamMembers.ToListAsync();
            return View(teamMembers);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {
            if (!ModelState.IsValid) return View(teamMember);

            if (!_fileService.IsImage(teamMember.Photo))
            {
                ModelState.AddModelError("Photo", "Fayl image formatında olmalıdır");
                return View(teamMember);
            }
            int maxSize = 60;

            if (!_fileService.CheckSize(teamMember.Photo, maxSize))
            {
                ModelState.AddModelError("Photo", $"Şəklin ölçüsü {maxSize} kb-dan çoxdur");
                return View(teamMember);
            }

            var filename = await _fileService.UploadAsync(teamMember.Photo);

            teamMember.PhotoName = filename;

             _dbContext.TeamMembers.Add(teamMember);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbTeamMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(tm => tm.Id == id);

            if (dbTeamMember == null) return NotFound();
            var model = new TeamMemberUpdateVM
            {
                Id = dbTeamMember.Id,
                Name = dbTeamMember.Name,
                Surname = dbTeamMember.Surname,
                PhotoName = dbTeamMember.PhotoName,
                Position = dbTeamMember.Position,
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Update(int id, TeamMemberUpdateVM model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid) return View(model);
            var dbTeamMember = await _dbContext.TeamMembers.FindAsync(id);

            _dbContext.TeamMembers.Update(dbTeamMember);
            await _dbContext.SaveChangesAsync();

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
                    ModelState.AddModelError("Photo", $"Şəkilin ölçüsü {maxSize} kb-dan kiçik olmalıdır");
                    return View(model);
                }

                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbTeamMember.PhotoName);
                _fileService.Delete(path);

                var filename = await _fileService.UploadAsync(model.Photo);
                dbTeamMember.PhotoName = filename;
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var dbTeamMember = await _dbContext.TeamMembers.FindAsync(id);
            if (dbTeamMember == null) return NotFound();
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbTeamMember.PhotoName);

            _fileService.Delete(path);

            _dbContext.Remove(dbTeamMember);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion



    }
}
