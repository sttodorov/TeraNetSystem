namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Payment : IEntityProtectedDelete
    {
        public Payment()
        {
            this.DateCreated = DateTime.Now;
            this.IsDeleted = false;
        }

        public int Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public Month PerMonth { get; set; }

        //[Required]
        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        //[Required]
        public string OfficeId { get; set; }

        public virtual Office Office { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
