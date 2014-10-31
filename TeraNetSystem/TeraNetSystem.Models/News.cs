namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class News
    {
        public News()
        {
            this.DateCreated = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Content { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}
