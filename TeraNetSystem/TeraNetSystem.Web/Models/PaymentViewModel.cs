namespace TeraNetSystem.Web.Models
{
    using System;
    using System.ComponentModel;
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

        [DisplayName("Date created:")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Per month:")]
        public Month PerMonth { get; set; }

        [DisplayName("Username:")]
        public string ClientUserName { get; set; }

        [DisplayName("Town:")]
        public string OfficeTown { get; set; }

        [DisplayName("Address:")]
        public string OfficeAddress { get; set; }
    }
}