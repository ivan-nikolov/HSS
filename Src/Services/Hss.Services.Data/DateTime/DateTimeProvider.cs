namespace Hss.Services.Data.DateTime
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
