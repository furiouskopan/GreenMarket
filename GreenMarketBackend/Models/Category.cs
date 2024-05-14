using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenMarketBackend.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        // ParentCategoryId for hierarchical categories
        public int? ParentCategoryId { get; set; }

        // Navigation property for the parent category
        public virtual Category ParentCategory { get; set; }

        // Navigation property for child categories if implementing hierarchical categories
        public virtual ICollection<Category> ChildCategories { get; set; }

        // Navigation property for products in this category
        public virtual ICollection<Product> Products { get; set; }
    }
}
