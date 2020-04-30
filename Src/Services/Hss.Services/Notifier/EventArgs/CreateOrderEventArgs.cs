namespace Hss.Services.Notifier.EventArgs
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CreateOrderEventArgs : IMapFrom<Order>
    {
        public string Id { get; set; }
    }
}
