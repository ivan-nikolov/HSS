namespace Hss.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class CategoriesListViewModel
    {
        public CategoriesListViewModel()
        {
            this.Categories = new HashSet<CategoryViewModel>();
        }

        public string Name { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
