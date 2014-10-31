namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class WorkTask
    {
        public WorkTask()
        {
            this.DateCreated = DateTime.Now;
            this.Compleated = false;
        }
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Location { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool Compleated { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? DateCompleated { get; set; }

        [Required]
        public int  NetworkManId { get; set; }

        public virtual ApplicationUser NetworkMan { get; set; }
    }
}
