namespace Hss.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class CategoriesListViewModel
    {
        public CategoriesListViewModel()
        {
            this.Categories = new HashSet<CategoryViewModel>();
        }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
