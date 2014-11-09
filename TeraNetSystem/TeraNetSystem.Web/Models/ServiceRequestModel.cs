using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeraNetSystem.Web.Models
{
    public class ServiceRequestModel
    {
        [Required]
        public int SubscriptionId { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
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
        [MinLength(5)]
        [MaxLength(20)]
        public string Phone { get; set; }

        // TODO: Add reCaptcha
        public List<SelectListItem> Towns { get; set; }

    }
}