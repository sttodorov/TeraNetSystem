namespace TeraNetSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Abonament
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int MB { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
