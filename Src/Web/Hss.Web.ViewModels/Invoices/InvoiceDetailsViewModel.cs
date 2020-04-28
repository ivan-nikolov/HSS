namespace Hss.Web.ViewModels.Invoices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class InvoiceDetailsViewModel : IMapFrom<Invoice>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ClientName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ClientEmail { get; set; }

        public decimal OrderServicePrice { get; set; }

        public decimal Discount { get; set; }

        public string Status { get; set; }

        public decimal NetAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public InvoiceAddressViewModel Address { get; set; }

        public ICollection<InvoiceJobViewModel> Jobs { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Invoice, InvoiceDetailsViewModel>()
                .ForMember(m => m.Jobs, e => e.MapFrom(i => i.Order.Jobs.OrderBy(j => j.StartDate)))
                .ForMember(m => m.ClientName, e => e.MapFrom(i => i.Client.FirstName + " " + i.Client.LastName));
        }
    }
}
