namespace TeraNetSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Town : IEntityProtectedDelete
    {
        private ICollection<ApplicationUser> users;
        private ICollection<Office> offices;

        public Town()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Offices = new HashSet<Office>();
            this.IsDeleted = false;
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [Index(IsUnique = true)]
        public string TownName { get; set; }

        public virtual ICollection<Office> Offices
        {
            get
            {
                return this.offices;
            }
            set
            {
                this.offices = value;
            }
        }

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

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
