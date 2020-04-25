namespace Hss.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateValidationAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Invalid date or time";
        private const int DefaultMinHour = 0;
        private const int DefaultMaxHour = 24;

        private string errorMessage;
        private int minHour;
        private int maxHour;

        public DateValidationAttribute(int minHour = DefaultMinHour, int maxHour = DefaultMaxHour, string errorMessage = DefaultErrorMessage)
        {
            this.minHour = minHour;
            this.maxHour = maxHour;
            this.errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (date < DateTime.UtcNow.Date 
                || date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday
                || date.Hour < this.minHour
                || date.Hour > this.maxHour)
            {
                return new ValidationResult(
                    this.errorMessage,
                    new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}
