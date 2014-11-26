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

    public class AdminOfficeViewModel: OfficeViewModel
    {
        public static Expression<Func<Office, AdminOfficeViewModel>> FromOffice
        {
            get
            {
                return o => new AdminOfficeViewModel
                {
                    Id = o.Id.ToString(),
                    Name = o.Name,
                    TownName = o.Town.TownName,
                    Address = o.Address,
                    Phone = o.Phone,
                    ImagePath = o.ImagePath,
                    Payments = o.Payments.AsQueryable()
                                .Select(PaymentViewModel.FromPayment)
                                .OrderByDescending(p => p.DateCreated)
                                .Take(3).ToList()
                };
            }
        }

        

        public ICollection<PaymentViewModel> Payments { get; set; }

        // TODO: Add payments count grouped by month.
    }
}