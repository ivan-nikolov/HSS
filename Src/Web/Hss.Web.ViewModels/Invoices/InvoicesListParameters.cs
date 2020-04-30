namespace Hss.Web.ViewModels.Invoices
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Hss.Data.Models.Enums;
    using Hss.Web.Infrastructure.Attributes;
    using Hss.Web.ViewModels.Common;

    public class InvoicesListParameters
    {
        public InvoicesListParameters()
        {
            this.ButtonValue = "Date &uarr;";
            this.AfterDate = DateTime.UtcNow.AddMonths(-1);
            this.BeforeDate = DateTime.UtcNow;
            this.InvoiceStatus = InvoiceStatus.Pending;
            this.IsDescendingOrder = true;
        }

        public string ButtonValue { get; set; }

        [ComparisonAttribute(nameof(BeforeDate), ComparisonType.LessThan, ErrorMessage = ValidationConstants.DateRangeErrorMessage)]
        [Display(Name = "From")]
        public DateTime AfterDate { get; set; }

        [ComparisonAttribute(nameof(AfterDate), ComparisonType.GreaterThanOrEqualTo, ErrorMessage = ValidationConstants.DateRangeErrorMessage)]
        [Display(Name = "To")]
        public DateTime BeforeDate { get; set; }

        public InvoiceStatus InvoiceStatus { get; set; }

        public bool IsDescendingOrder { get; set; }
    }
}
