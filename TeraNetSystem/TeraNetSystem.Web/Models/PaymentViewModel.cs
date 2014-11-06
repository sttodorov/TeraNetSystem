namespace TeraNetSystem.Web.Models
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using TeraNetSystem.Models;

    public class PaymentViewModel
    {
        public static Expression<Func<Payment, PaymentViewModel>> FromPayment
        {
            get
            {
                return p => new PaymentViewModel
                {
                    Id = p.Id,
                    DateCreated = p.DateCreated,
                    PerMonth = p.PerMonth,
                    OfficeTown = p.Office.Town.TownName,
                    OfficeAddress = p.Office.Address,
                    ClientUserName = p.Client.UserName
                };
            }
        }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public Month PerMonth { get; set; }

        public string ClientUserName { get; set; }

        public string OfficeTown { get; set; }

        public string OfficeAddress { get; set; }
    }
}