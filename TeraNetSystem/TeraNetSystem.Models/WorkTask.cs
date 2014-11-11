namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class WorkTask : IEntityProtectedDelete
    {
        public WorkTask()
        {
            this.Id = Guid.NewGuid();
            this.DateCreated = DateTime.Now;
            this.Compleated = false;
            this.IsDeleted = false;
        }
        public Guid Id { get; set; }
        
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool Compleated { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleated { get; set; }

        public string  NetworkManId { get; set; }

        public virtual ApplicationUser NetworkMan { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
