namespace Hss.Web.Components.RazorComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;

    using Hss.Services.Notifications;

    using Microsoft.AspNetCore.Components;

    public partial class BlazoredToasts
    {
        private string positionClass = string.Empty;

        [Inject]
        public IToastService ToastService { get; set; }

        [Parameter]
        public string InfoClass { get; set; }

        [Parameter]
        public string InfoIconClass { get; set; }

        [Parameter]
        public string SuccessClass { get; set; }

        [Parameter]
        public string SuccessIconClass { get; set; }

        [Parameter]
        public string WarningClass { get; set; }

        [Parameter]
        public string WarningIconClass { get; set; }

        [Parameter]
        public string ErrorClass { get; set; }

        [Parameter]
        public string ErrorIconClass { get; set; }

        [Parameter]
        public ToastPosition Position { get; set; } = ToastPosition.TopRight;

        [Parameter]
        public int Timeout { get; set; } = 5;

        public List<ToastInstance> ToastList { get; set; } = new List<ToastInstance>();

        public async Task RemoveToast(string id)
        {
            await this.InvokeAsync(() =>
            {
                var toastInstance = this.ToastList.SingleOrDefault(t => t.Id == id);
                this.ToastList.Remove(toastInstance);
                this.StateHasChanged();
            });
        }

        protected override void OnInitialized()
        {
            this.ToastService.OnShow += this.ShowToast;
            this.positionClass = $"position-{this.Position.ToString().ToLower()}";
        }

        private ToastSettings BuildToastSettings(ToastLevel level, string message, string heading)
        {
            var toastSettings = level switch
            {
                ToastLevel.Info => new ToastSettings(string.IsNullOrEmpty(heading) ? "Info" : heading, message, "blazored-toast-info", this.InfoClass, this.InfoIconClass),
                ToastLevel.Success => new ToastSettings(string.IsNullOrEmpty(heading) ? "Success" : heading, message, "blazored-toast-success", this.SuccessClass, this.SuccessIconClass),
                ToastLevel.Warning => new ToastSettings(string.IsNullOrEmpty(heading) ? "Warning" : heading, message, "blazored-toast-warning", this.WarningClass, this.WarningIconClass),
                ToastLevel.Error => new ToastSettings(string.IsNullOrEmpty(heading) ? "Error" : heading, message, "blazored-toast-error", this.ErrorClass, this.ErrorIconClass),
                _ => throw new ArgumentException("Invalid Toast Level."),
            };

            return toastSettings;
        }

        private async Task ShowToast(ToastLevel level, string message, string heading)
        {
            await this.InvokeAsync(() =>
            {
                var settings = this.BuildToastSettings(level, message, heading);
                var toast = new ToastInstance()
                {
                    ToastSettings = settings,
                };

                this.ToastList.Add(toast);

                var timeout = this.Timeout * 1000;
                var toastTimer = new Timer(timeout);
                toastTimer.Elapsed += (sender, args) =>
                {
                    this.RemoveToast(toast.Id).GetAwaiter().GetResult();
                };

                toastTimer.AutoReset = false;
                toastTimer.Start();
                this.StateHasChanged();
            });

        }
    }
}
