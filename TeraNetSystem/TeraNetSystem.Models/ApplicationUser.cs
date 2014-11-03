namespace TeraNetSystem.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Payment> payments;

        public ApplicationUser()
            :base()
        {
            this.Payments = new HashSet<Payment>();
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
        public int AbonamentId { get; set; }

        public virtual Abonament Abonament { get; set; }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


    }
}
