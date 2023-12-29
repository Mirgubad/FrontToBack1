using FrontToBack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Areas.Admin.ViewModels.CategoryComponent
{
    public class CategoryComponentUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? FilePath { get; set; }
        public Category? Category { get; set; }
        [Required]
        [Display(Name = "Categories")]
        public int CategoryId { get; set; }
        public IFormFile? Photo { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
