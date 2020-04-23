namespace Hss.Web.ViewModels.Common
{
    public class ValidationConstants
    {
        public const string StringLengthErrorMessage = "{0} must be less then {1} and more then {2} characters long.";

        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 30;

        public const int ServiceNameMinLength = 3;
        public const int ServiceNamehMaxLength = 30;

        public const int DescriptionMaxLength = 5000;

        public const int CountryNameMinLength = 4;
        public const int CountryNameMaxLength = 90;

        public const int CityNameMinLength = 2;
        public const int CityNameMaxLength = 200;

        public const int TeamNameMinLength = 3;
        public const int TeamNameMaxLength = 50;
    }
}
