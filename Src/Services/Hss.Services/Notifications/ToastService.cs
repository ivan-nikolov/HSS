namespace Hss.Services.Notifications
{
    using System;
    using System.Threading.Tasks;

    public class ToastService : IToastService
    {
        public event Func<ToastLevel, string, string, Task> OnShow;

        public void ShowError(string message, string heading = "")
            => this.ShowToast(ToastLevel.Error, message, heading);

        public void ShowInfo(string message, string heading = "")
            => this.ShowToast(ToastLevel.Info, message, heading);

        public void ShowSuccess(string message, string heading = "")
            => this.ShowToast(ToastLevel.Success, message, heading);

        public void ShowToast(ToastLevel level, string message, string heading = "")
        {
            this.OnShow?.Invoke(level, message, heading);
        }

        public void ShowWarning(string message, string heading = "")
            => this.ShowToast(ToastLevel.Warning, message, heading);
    }
}
