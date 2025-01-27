﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Models
{
    public class CategoryComponent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? FilePath { get; set; }
        public Category? Category { get; set; }
        [Required]
        public int CategoryId { get; set; }

   

    }
}
