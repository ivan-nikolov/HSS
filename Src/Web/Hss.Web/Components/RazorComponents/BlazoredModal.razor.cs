namespace Hss.Web.Components.RazorComponents
{
    using System;

    using Hss.Services.BlazorModal;
    using Microsoft.AspNetCore.Components;

    public partial class BlazoredModal : IDisposable
    {
        private const string DefaultStyle = "blazored-modal";
        private const string DefaultPosition = "blazored-modal-center";

        private bool componentDisableBackgroundCancel;

        private bool componentHideHeader;

        private bool componentHideCloseButton;

        private string componentPosition;

        private string componentStyle;

        private bool isVisibal;

        private string title;

        private RenderFragment content;

        private ModalParameters parameters;

        [Parameter]
        public bool HideHeader { get; set; }

        [Parameter]
        public bool HideCloseButton { get; set; }

        [Parameter]
        public bool DisableBackgroundCancel { get; set; }

        [Parameter]
        public string Position { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Inject]
        private IModalService ModalService { get; set; }

        public void SetTitle(string title)
        {
            this.title = title;
            this.StateHasChanged();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void OnInitialized()
        {
            ((ModalService)this.ModalService).OnShow += this.ShowModal;
            ((ModalService)this.ModalService).CloseModal += this.CloseModal;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ((ModalService)this.ModalService).OnShow -= this.ShowModal;
                ((ModalService)this.ModalService).CloseModal -= this.CloseModal;
            }
        }

        private async void ShowModal(object sender, OnShowEventArgs eventArgs)
        {
            this.title = eventArgs.Title;
            this.content = eventArgs.Content;
            this.parameters = eventArgs.Parameters;
            this.isVisibal = true;
            this.SetModalOptions(eventArgs.Options);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async void CloseModal(object sender, EventArgs eventArgs)
        {
            this.title = string.Empty;
            this.content = null;
            this.componentStyle = string.Empty;
            this.isVisibal = false;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private void HandleBackgroundClick()
        {
            if (this.componentDisableBackgroundCancel)
            {
                return;
            }

            this.ModalService.Cancel();
        }

        private void SetModalOptions(ModalOptions options)
        {
            this.componentHideHeader = this.HideHeader;
            if (options.HideHeader.HasValue)
            {
                this.componentHideHeader = options.HideHeader.Value;
            }

            this.componentHideCloseButton = this.HideCloseButton;
            if (options.HideCloseButton.HasValue)
            {
                this.componentHideCloseButton = options.HideCloseButton.Value;
            }

            this.componentDisableBackgroundCancel = this.DisableBackgroundCancel;
            if (options.DisableBackGroundCancel.HasValue)
            {
                this.componentDisableBackgroundCancel = options.DisableBackGroundCancel.Value;
            }

            this.componentPosition = string.IsNullOrWhiteSpace(this.Position) ? this.Position : options.Position;
            if (string.IsNullOrWhiteSpace(this.componentPosition))
            {
                this.componentPosition = DefaultPosition;
            }

            this.componentStyle = string.IsNullOrWhiteSpace(this.Style) ? this.Style : options.Style;
            if (string.IsNullOrWhiteSpace(this.componentStyle))
            {
                this.componentStyle = DefaultStyle;
            }
        }
    }
}
