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
                    Subscription = new SubscriptionViewModel
                    {
                        Id = a.Subscription.Id,
                        SubscriptionName = a.Subscription.SubscriptionName,
                        Price = a.Subscription.Price,
                        DownloadSpeed = a.Subscription.DownloadSpeed,
                        UploadSpeed = a.Subscription.UploadSpeed,
                        Description = a.Subscription.Description
                    },
                    ContractNumber = a.ContractNumber,
                    RegisteredFrom = a.DateRegistered,
                    Office = new OfficeViewModel
                    {
                        Id = a.Office.Id.ToString(),
                        TownName = a.Office.Town.TownName,
                        Address = a.Office.Address,
                        Name = a.Office.Name,
                        Phone = a.Office.Phone,
                    }
                };
            }
        }
        public string Id { get; set; }

        public bool HasPassword { get; set; }

        public ICollection<IdentityUserLogin> Logins { get; set; }

        [DisplayName("Phone number: ")]
        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        [DisplayName("Fullname: ")]
        public string FullName { get; set; }

        [DisplayName("Town: ")]
        public string TownName { get; set; }

        [DisplayName("Address: ")]
        public string Address { get; set; }

        [DisplayName("Username: ")]
        public string UserName { get; set; }

        [DisplayName("E-mail: ")]
        public string Email { get; set; }

        [DisplayName("Contract №: ")]
        public string ContractNumber { get; set; }

        [DisplayName("Member from: ")]
        public DateTime RegisteredFrom { get; set; }

        public OfficeViewModel Office { get; set; }

        public SubscriptionViewModel Subscription { get; set; }
    }
}