namespace Hss.Services.Notifier
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Notifier
    {
        private readonly List<string> bookingsInProcess = new List<string>();

        public event Func<List<string>, Task> OnProcessPendingBookind;

        public event Func<List<string>, Task> OnCancelProcessingBooking;

        public event Func<Task> OnCreatedOrder;

        public event Func<Task> OnOrderStatusChange;

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

            if (this.OnCancelProcessingBooking != null)
            {
                await this.OnCancelProcessingBooking?.Invoke(this.bookingsInProcess);
            }
        }

        public async Task OrderCreated()
        {
            if (this.OnCreatedOrder != null)
            {
                await this.OnCreatedOrder?.Invoke();
            }
        }

        public async Task OrderStatusChanged()
        {
            if (this.OnOrderStatusChange != null)
            {
                await this.OnOrderStatusChange.Invoke();
            }
        }
    }
}
