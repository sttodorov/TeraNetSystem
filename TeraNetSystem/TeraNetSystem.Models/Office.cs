namespace TeraNetSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Office : IEntityProtectedDelete
    {
        private ICollection<Payment> payments;
        private ICollection<ApplicationUser> staff;

        public Office()
            :base()
        {
            this.Id = Guid.NewGuid();
            this.Payments = new HashSet<Payment>();
            this.Staff = new HashSet<ApplicationUser>();
            this.IsDeleted = false;
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        //[Required]
        [DefaultValue("~/Content/images/office.png")]
        public string ImagePath { get; set; }

        public virtual ICollection<ApplicationUser> Staff
        {
            get
            {
                return this.staff;
            }
            set
            {
                this.staff = value;
            }
        }

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
    }
}
