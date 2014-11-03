namespace TeraNetSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Abonament
    {
        private ICollection<ApplicationUser> users;

        public Abonament()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string AbonamentName { get; set; }

        [Required]
        public int MB { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users
        {
            get
            {
                return this.users;
            }
            set
            {
                this.users = value;
            }
        }
    }
}
