using System.ComponentModel.DataAnnotations;

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

        // ParentCategoryId if implementing hierarchical categories

        // Navigation property for child categories if implementing hierarchical categories
        public virtual ICollection<Category> ChildCategories { get; set; }

        // Navigation property for products in this category
        public virtual ICollection<Product> Products { get; set; }
    }
}
