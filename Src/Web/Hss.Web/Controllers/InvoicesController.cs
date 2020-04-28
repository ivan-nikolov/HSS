namespace Hss.Web.Controllers
{
    using System.Threading.Tasks;

    using Hss.Services.Data.Invoices;
    using Hss.Web.ViewModels.Invoices;
    using Microsoft.AspNetCore.Mvc;

    public class InvoicesController : BaseController
    {
        private readonly IInvoicesService invoicesService;

        public InvoicesController(IInvoicesService invoicesService)
        {
            this.invoicesService = invoicesService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var invoice = await this.invoicesService.GetByIdAsync<InvoiceDetailsViewModel>(id);
            if (invoice == null)
            {
                return this.NotFound();
            }

            return this.View(invoice);
        }
    }
}
