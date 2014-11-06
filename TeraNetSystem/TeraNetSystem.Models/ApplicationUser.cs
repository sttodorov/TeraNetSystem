namespace TeraNetSystem.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System;

    public class ApplicationUser : IdentityUser, IEntityProtectedDelete
    {
        private ICollection<Payment> payments;

        public ApplicationUser()
            :base()
        {
            this.Payments = new HashSet<Payment>();
            this.DateRegistered = DateTime.Now;
            IsDeleted = false;
        }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }
        
        [Required]
        public DateTime DateRegistered { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [MinLength(5)]
        [MaxLength(20)]
        public string ContractNumber { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        public virtual Subscription Subscription { get; set; }

        public virtual ICollection<Payment> Payments
        {
            get
            {
                return this.payments;
            }
            set
            {
                this.payments = value;
            }
        }

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
