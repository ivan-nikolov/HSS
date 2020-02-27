// ReSharper disable VirtualMemberCallInConstructor
namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.Orders = new HashSet<Order>();
            this.Contracts = new HashSet<Contract>();
            this.Invoices = new HashSet<Invoice>();

            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TeamId { get; set; }

        public Team Team { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
