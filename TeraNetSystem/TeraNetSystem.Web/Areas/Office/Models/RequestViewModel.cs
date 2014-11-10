using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Areas.Office.Models
{
    public class RequestViewModel
    {
        public static Expression<Func<Request, RequestViewModel>> FromRequest
        {
            get
            {
                return r => new RequestViewModel
                {
                    Id = r.Id.ToString(),
                    SubscriptionName = r.Subscription.SubscriptionName,
                    TownName = r.Town.TownName,
                    Address = r.Address,
                    FullName = string.Format("{0} {1}",r.FirstName, r.LastName),
                    PhoneNumber = r.PhoneNumber
                };
            }
        }

        public string Id { get; set; }

        [DisplayName("Town: ")]
        public string TownName { get; set; }

        [DisplayName("Subscription plan: ")]
        public string SubscriptionName { get; set; }

        [DisplayName("Address: ")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("Full name: ")]
        public string FullName { get; set; }

        [DisplayName("Phone number: ")]
        public string PhoneNumber { get; set; }
    }
}