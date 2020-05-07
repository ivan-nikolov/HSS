namespace Hss.Services.Notifications
{
    public class ToastSettings
    {
        public ToastSettings(
            string heading,
            string message,
            string baseClass,
            string additionalClasses,
            string iconClass)
        {
            this.Heading = heading;
            this.Message = message;
            this.BaseClass = baseClass;
            this.AdditionalClasses = additionalClasses;
            this.IconClass = iconClass;
        }

        public string Heading { get; set; }

        public string Message { get; set; }

        public string BaseClass { get; set; }

        public string AdditionalClasses { get; set; }

        public string IconClass { get; set; }
    }
}