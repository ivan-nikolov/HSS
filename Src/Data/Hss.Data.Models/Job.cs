﻿namespace Hss.Data.Models
{
    using System;

    using Hss.Data.Common.Models;

    public class Job : BaseDeletableModel<string>
    {
        public Job()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
