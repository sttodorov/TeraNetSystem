﻿namespace TeraNetSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Subscription : IEntityProtectedDelete
    {
        private ICollection<ApplicationUser> users;

        public Subscription()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.IsDeleted = false;
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string SubscriptionName { get; set; }

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

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
