namespace TeraNetSystem.Web.Areas.Office.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Models;

    public class OfficeSubscriptionViewModel : SubscriptionViewModel
    {
        public static Expression<Func<Subscription, OfficeSubscriptionViewModel>> FromSubscription
        {
            get
            {
                return s => new OfficeSubscriptionViewModel
                {
                    Id = s.Id,
                    SubscriptionName = s.SubscriptionName,
                    Description = s.Description,
                    DownloadSpeed = s.DownloadSpeed,
                    UploadSpeed = s.UploadSpeed,
                    Price = s.Price,
                    Users =  s.Users.AsQueryable()
                                .Select(ApplicationUserViewModel.FromUser)
                                .ToList()
                };
            }
        }

        public List<ApplicationUserViewModel> Users { get; set; }
    }
}