namespace TeraNetSystem.Web.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
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
                    Office = new OfficeViewModel
                    {
                        Id = p.Office.Id.ToString(),
                        Address = p.Office.Address,
                        TownName = p.Office.Town.TownName,
                        Name = p.Office.Name,
                        Phone = p.Office.Phone
                    },
                    Client = new ApplicationUserViewModel
                    {
                        Id= p.Client.Id,
                        UserName = p.Client.UserName,
                        TownName  = p.Client.Town.TownName,
                        Address = p.Client.Address,
                        FullName = p.Client.FirstName + " " + p.Client.LastName,
                        Subscription = new SubscriptionViewModel
                        {
                            SubscriptionName = p.Client.Subscription.SubscriptionName,
                            Price = p.Client.Subscription.Price
                        }
                    },
                };
            }
        }

        public int Id { get; set; }

        [DisplayName("Date created:")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Per month:")]
        [Required]
        public Month PerMonth { get; set; }

        public ApplicationUserViewModel Client { get; set; }

        public OfficeViewModel Office { get; set; }

    }
}