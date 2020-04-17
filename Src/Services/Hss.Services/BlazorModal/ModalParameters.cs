namespace Hss.Services.BlazorModal
{
    using System.Collections.Generic;

    public class ModalParameters
    {
        private readonly IDictionary<string, object> parameters;

        public ModalParameters()
        {
            this.parameters = new Dictionary<string, object>();
        }

        public void Add(string parameterName, object parameter)
        {
            this.parameters[parameterName] = parameter;
        }

        public T Get<T>(string parameterName)
        {
            if (!this.parameters.ContainsKey(parameterName))
            {
                throw new KeyNotFoundException("Parameter not found!");
            }

            return (T)this.parameters[parameterName];
        }

        public T TryGet<T>(string parameterName)
        {
            if (this.parameters.ContainsKey(parameterName))
            {
                return (T)this.parameters[parameterName];
            }

            return default;
        }
    }
}
