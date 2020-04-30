namespace Hss.Web.Controllers
{
    using System.Threading.Tasks;

    using Hss.Services.Data.Orders;
    using Hss.Web.ViewModels.Orders;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
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

            return this.Redirect("/Dashboard");
        }
    }
}
