namespace GreenMarketBackend.Models.ViewModels
{
    public class ProductFilterViewModel
    {
        public IEnumerable<Category> ParentCategories { get; set; }
        public IEnumerable<Category> ChildCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int? SelectedParentCategoryId { get; set; }
        public int? SelectedChildCategoryId { get; set; }
    }
}
