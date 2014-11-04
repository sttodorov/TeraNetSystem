namespace TeraNetSystem.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;

    using Microsoft.AspNet.Identity;

    using TeraNetSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUserViewModel
    {
        public static Expression<Func<ApplicationUser, ApplicationUserViewModel>> FromUser
        {
            get
            {
                return a => new ApplicationUserViewModel
                {
                    Id = a.Id,
                    HasPassword = a.PasswordHash != null,
                    PhoneNumber = a.PhoneNumber,
                    TwoFactor = a.TwoFactorEnabled,
                    Logins = a.Logins,
                    BrowserRemembered = a.TwoFactorEnabled,
                    FullName = a.FirstName + " " + a.LastName,
                    TownName = a.Town.TownName,
                    Address = a.Address,
                    UserName = a.UserName,
                    Email = a.Email,
                    SubscriptionPlan = a.Subscription.SubscriptionName,
                    ContractNumber = a.ContractNumber,
                    RegisteredFrom = a.DateRegistered
                };
            }
        }
        public string Id { get; set; }

        public bool HasPassword { get; set; }

        public ICollection<IdentityUserLogin> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public string FullName { get; set; }

        public string TownName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string SubscriptionPlan { get; set; }

        public string ContractNumber { get; set; }

        public DateTime RegisteredFrom { get; set; }
    }
}