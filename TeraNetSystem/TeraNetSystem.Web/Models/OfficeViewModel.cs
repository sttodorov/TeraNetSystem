using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Models
{
    public class OfficeViewModel
    {
        public static Expression<Func<Office, OfficeViewModel>> FromOffice
        {
            get
            {
                return o => new OfficeViewModel
                {
                    Id = o.Id.ToString(),
                    TownName = o.Town.TownName,
                    Address = o.Address,
                    Phone = o.Phone,
                    ImagePath = o.ImagePath
                };
            }
        }

        public string Id { get; set; }

        [DisplayName("Town:")]
        [Required]
        public string TownName { get; set; }

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

        [DisplayName("Photo:")]
        [DataType(DataType.Upload)]
        public string ImagePath { get; set; }
    }
}