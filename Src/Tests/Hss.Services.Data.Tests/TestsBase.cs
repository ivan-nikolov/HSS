namespace Hss.Services.Data.Tests
{
    using System.Reflection;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;
    using Hss.Web.ViewModels;

    public class TestsBase
    {
        public TestsBase()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(CategoryServiceModel).GetTypeInfo().Assembly);
        }
    }
}
