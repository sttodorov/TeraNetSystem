namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Payment
    {
        public Payment()
        {
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public Month PerMonth { get; set; }

        [Required]
        public int ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        [Required]
        public string OfficeId { get; set; }

        public virtual Office Office { get; set; }

    }
}
