namespace Hss.Web.ViewModels.Invoices
{
    using System;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class InvoiceJobViewModel : IMapFrom<Job>
    {
        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }
    }
}
