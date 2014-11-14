    namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class News : IEntityProtectedDelete
    {
        public News()
        {
            this.DateCreated = DateTime.Now;
            this.IsDeleted = false;
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
        public string ImagePath { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
