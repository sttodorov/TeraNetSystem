namespace TeraNetSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Office
    {
        private ICollection<Payment> payments;

        public Office()
            :base()
        {
            this.Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

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
    }
}
