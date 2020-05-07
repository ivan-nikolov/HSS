namespace Hss.Services.Notifications
{
    using System;

    public class ToastInstance
    {
        public ToastInstance()
        {
            this.Id = Guid.NewGuid().ToString();
            this.TimeStamp = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public ToastSettings ToastSettings { get; set; }
    }
}
