namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Request
    {
        public Request()
        {
            this.Id = new Guid();
        }

        public Guid Id { get; set; }

        [Required]
        public int AbonamentId { get; set; }

        public virtual Abonament Abonament { get; set; }

        [Required]
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
