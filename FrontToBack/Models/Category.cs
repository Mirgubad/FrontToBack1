using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Mütləq doldurulmalıdır"), MinLength(4,ErrorMessage ="3 hərfdən kiçik olmaz")]
        public string Title { get; set; }
        public List<CategoryComponent>? CategoryComponents { get; set; }

    }
}
