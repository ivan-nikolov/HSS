namespace Hss.Web.Controllers
{
    using System.Threading.Tasks;

    using Hss.Services.Data.Orders;
    using Hss.Services.Notifier;
    using Hss.Web.ViewModels.Orders;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly Notifier notifier;

        public OrdersController(IOrdersService ordersService, Notifier notifier)
        {
            this.ordersService = ordersService;
            this.notifier = notifier;
        }

        public IActionResult BookService()
        {
            return this.View();
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var order = await this.ordersService.GetByIdAsync<CancelOrderViewModel>(id);
            if (order == null)
            {
                return this.NotFound();
            }

            return this.View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(CancelOrderViewModel input)
        {
            if (!this.ordersService.CheckIfOrderExists(input.Id))
            {
                return this.NotFound();
            }

            await this.ordersService.CancelAsync(input.Id);
            await this.notifier.OrderStatusChanged(input.Id);

            return this.Redirect("/Accounts/Dashboard");
        }
    }
}
