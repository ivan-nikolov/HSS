namespace Hss.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public enum ComparisonType
    {
        LessThan = 1,
        LessThanOrEqualTo = 2,
        EqualTo = 3,
        GreaterThan = 4,
        GreaterThanOrEqualTo,
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ComparisonAttribute : ValidationAttribute
    {
        private readonly string comparisonProperty;
        private readonly ComparisonType comparisonType;

        public ComparisonAttribute(string comparisonProperty, ComparisonType comparisonType)
        {
            this.comparisonProperty = comparisonProperty;
            this.comparisonType = comparisonType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            this.ErrorMessage = this.ErrorMessageString;

            if (value.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("value has not implemented IComparable interface");
            }

            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(this.comparisonProperty);

            if (property == null)
            {
                throw new ArgumentException("Comparison property with this name not found");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("Comparison property has not implemented IComparable interface");
            }

            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
            {
                throw new ArgumentException("The properties types must be the same");
            }

            bool compareToResult;

            switch (this.comparisonType)
            {
                case ComparisonType.LessThan:
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) >= 0;

                    break;

                case ComparisonType.LessThanOrEqualTo:
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) > 0;

                    break;

                case ComparisonType.EqualTo:
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) != 0;

                    break;

                case ComparisonType.GreaterThan:
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) <= 0;

                    break;

                case ComparisonType.GreaterThanOrEqualTo:
                    compareToResult = currentValue.CompareTo((IComparable)comparisonValue) < 0;

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return compareToResult ? new ValidationResult(string.Format(this.ErrorMessage, validationContext.MemberName, this.comparisonProperty), new string[] { this.comparisonProperty, validationContext.MemberName }) : ValidationResult.Success;
        }
    }
}
