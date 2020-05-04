namespace Hss.Web.ViewModels.Components.RazorComponents.Orders
{
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class ServiceInputModel : IMapFrom<Service>
    {
        public int Id { get; set; }

        public bool IsRecurrent { get; set; }

        public string Name { get; set; }

        public int DurationInHours { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; }
    }
}
