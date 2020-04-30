namespace Hss.Services.Notifier
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Notifier.EventArgs;

    public class Notifier
    {
        private readonly List<string> bookingsInProcess = new List<string>();

        public event Func<List<string>, Task> OnProcessPendingBookind;

        public event Func<List<string>, Task> OnCancelProcessingBooking;

        public event EventHandler<CreateOrderEventArgs> OnCreatedOrder;

        public async Task ProcessingBooking(string orderId)
        {
            if (!this.bookingsInProcess.Contains(orderId))
            {
                this.bookingsInProcess.Add(orderId);
            }

            await this.OnProcessPendingBookind?.Invoke(this.bookingsInProcess);
        }

        public async Task CancelProcessingBookings(string orderId)
        {
            if (this.bookingsInProcess.Contains(orderId))
            {
                this.bookingsInProcess.Remove(orderId);
            }

            await this.OnCancelProcessingBooking?.Invoke(this.bookingsInProcess);
        }

        public void OrderCreated(object sender, CreateOrderEventArgs eventArgs)
        {
            this.OnCreatedOrder?.Invoke(sender, eventArgs);
        }
    }
}
