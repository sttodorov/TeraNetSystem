namespace TeraNetSystem.Web.Areas.Office.Models
{
    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Models;

    public class TaskCreateViewModel
    {
        public static Expression<Func<Request, TaskCreateViewModel>> FromRequest
        {
            get
            {
                return r => new TaskCreateViewModel
                {
                    Id = r.Id.ToString(),
                    SubscriptionName = r.Subscription.SubscriptionName,
                    TownName = r.Town.TownName,
                    Address = r.Address,
                    FullName = r.FirstName + " " + r.LastName,//string.Format("{0} {1}", r.FirstName, r.LastName),
                    PhoneNumber = r.PhoneNumber
                };
            }
        }

        public string Id { get; set; }

        [DisplayName("Networkman: ")]
        public string NetworkerId { get; set; }

        [DisplayName("Town: ")]
        public string TownName { get; set; }

        [DisplayName("Subscription plan: ")]
        public string SubscriptionName { get; set; }

        [DisplayName("Address: ")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [DisplayName("Description: ")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("Full name: ")]
        public string FullName { get; set; }

        [DisplayName("Phone number: ")]
        public string PhoneNumber { get; set; }

        public List<SelectListItem> Netwrokers { get; set; }
    }
}