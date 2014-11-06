namespace TeraNetSystem.Web.Areas.Administration.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Models;

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

        public string TownName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string ImagePath { get; set; }

        public ICollection<PaymentViewModel> Payments { get; set; }

        // TODO: Add payments count grouped by month.
    }
}