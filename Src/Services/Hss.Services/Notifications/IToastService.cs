namespace Hss.Services.Notifications
{
    using System;
    using System.Threading.Tasks;

    public interface IToastService
    {
        public event Func<ToastLevel, string, string, Task> OnShow;

        public void ShowInfo(string message, string heading = "");

        public void ShowSuccess(string message, string heading = "");

        public void ShowWarning(string message, string heading = "");

        public void ShowError(string message, string heading = "");

        public void ShowToast(ToastLevel level, string message, string heading = "");
    }
}
