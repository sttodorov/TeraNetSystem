﻿namespace TeraNetSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Request : IEntityProtectedDelete
    {
        public Request()
        {
            this.Id = Guid.NewGuid();
            this.IsDeleted = false;
            this.Approved = false;
        }

        public Guid Id { get; set; }

        [Required]
        public int SubscriptionId { get; set; }

        public virtual Subscription Subscription { get; set; }

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

        public bool Approved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DateDeleted { get; set; }
    }
}
