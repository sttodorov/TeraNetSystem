using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Areas.Administration.Models
{
    public class OfficeCreateModel
    {
        
        [DisplayName("Town:")]
        public int TownId { get; set; }

        [DisplayName("Address:")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("Phone:")]
        [DataType(DataType.PhoneNumber)]
        [UIHint("Text")]
        public string Phone { get; set; }

        [DisplayName("Image:")]
        public HttpPostedFileBase Image { get; set; }
    }
}