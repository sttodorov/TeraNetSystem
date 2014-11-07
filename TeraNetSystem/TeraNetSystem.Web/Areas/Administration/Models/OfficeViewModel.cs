namespace TeraNetSystem.Web.Areas.Administration.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Models;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

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
                    ImagePath = o.ImagePath,
                    Payments = o.Payments.Select(p => new PaymentViewModel
                    {
                        Id = p.Id,
                        DateCreated = p.DateCreated,
                        PerMonth = p.PerMonth,
                        OfficeTown = p.Office.Town.TownName,
                        OfficeAddress = p.Office.Address,
                        ClientUserName = p.Client.UserName
                    }).ToList()
                };
            }
        }

        public string Id { get; set; }

        [DisplayName("Town:")]
        public string TownName { get; set; }

        [DisplayName("Address:")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("Phone:")]
        [DataType(DataType.PhoneNumber)]
        [UIHint("Text")]
        public string Phone { get; set; }

        [DisplayName("Photo:")]
        [DataType(DataType.Upload)]
        public string ImagePath { get; set; }

        public ICollection<PaymentViewModel> Payments { get; set; }

        // TODO: Add payments count grouped by month.
    }
}