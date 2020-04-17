namespace Hss.Services.BlazorModal
{
    using System;

    using Microsoft.AspNetCore.Components;

    public interface IModalService
    {
        event EventHandler<ModalResult> OnClose;

        void Show<T>(string title, ModalParameters parameters)
            where T : ComponentBase;

        void Show<T>(string title, ModalParameters parameters = null, ModalOptions options = null)
            where T : ComponentBase;

        void Show(Type contentComponent, string title, ModalParameters parameters, ModalOptions options);

        void Close(ModalResult modalResult);

        void Cancel();
    }
}
