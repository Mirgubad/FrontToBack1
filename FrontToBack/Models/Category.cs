using System.ComponentModel.DataAnnotations;

namespace FrontToBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Adivi yaz!!!"), MinLength(4,ErrorMessage ="4 herfden kicik olmaz")]
        public string Title { get; set; }
        public List<CategoryComponent>? CategoryComponents { get; set; }

    }
}
