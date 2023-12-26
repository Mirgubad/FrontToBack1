using FrontToBack.DAL;
using FrontToBack.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrontToBack.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ContactController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var contactIntro = await _appDbContext.ContactIntro.FirstOrDefaultAsync();

            ContactIndexViewModel model = new ContactIndexViewModel
            {
                ContactIntro = contactIntro
            };

            return View(model);
        }
    }
}
