namespace Hss.Services.Data.DateTime
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }
}
