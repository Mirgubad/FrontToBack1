using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Areas.Admin.ViewModels.CategoryComponent
{
    public class CategoryComponentCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? FilePath { get; set; }
        public Category? Category { get; set; }
        [Required]
        [Display(Name = "Categories")]
        public int CategoryId { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }
}
