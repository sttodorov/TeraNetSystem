using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Areas.Administration.Models
{
    public class OfficeCreateModel
    {

        [DisplayName("Town:")]
        [Required]
        [Range(1, int.MaxValue)]
        public int TownId { get; set; }

        [DisplayName("Office name: ")]
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Address:")]
        [DataType(DataType.MultilineText)]
        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string Address { get; set; }

        [DisplayName("Phone:")]
        [DataType(DataType.PhoneNumber)]
        [UIHint("Text")]
        [Required]
        [MinLength(4)]
        public string Phone { get; set; }

        [DisplayName("Image:")]
        public HttpPostedFileBase Image { get; set; }

        public List<SelectListItem> Towns { get; set; }
    }
}