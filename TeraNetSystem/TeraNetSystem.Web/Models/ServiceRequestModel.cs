using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Town: ")]
        public int TownId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [DisplayName("Address: ")]
        public string Address { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [DisplayName("First name: ")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        [DisplayName("Last name: ")]
        public string LastName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        [DisplayName("Phone: ")]
        public string Phone { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        [DisplayName("Validation Code: ")]
        public string RequstCaptcha { get; set; }

        public List<SelectListItem> Towns { get; set; }

    }
}