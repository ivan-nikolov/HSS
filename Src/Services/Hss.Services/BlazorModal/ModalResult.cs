namespace Hss.Services.BlazorModal
{
    using System;

    public class ModalResult
    {
        public ModalResult(object data, Type resultType, bool cancelled, Type modelType)
        {
            this.Data = data;
            this.DataType = resultType;
            this.Cancelled = cancelled;
            this.ModalType = modelType;
        }

        public object Data { get; }

        public Type DataType { get; }

        public bool Cancelled { get; }

        public Type ModalType { get; set; }

        public static ModalResult Ok<T>(T result)
            => Ok(result, default);

        public static ModalResult Ok<T>(T result, Type modalType)
            => new ModalResult(result, typeof(T), false, modalType);

        public static ModalResult Cancel()
            => Cancel(default);

        public static ModalResult Cancel(Type modelType)
            => new ModalResult(default, typeof(object), true, modelType);
    }
}