using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Models
{
    public class SubscriptionViewModel
    {
        public static Expression<Func<Subscription, SubscriptionViewModel>> FromSubscription
        {
            get
            {
                return s => new SubscriptionViewModel
                {
                    Id = s.Id,
                    SubscriptionName = s.SubscriptionName,
                    Description = s.Description,
                    DownloadSpeed = s.DownloadSpeed,
                    UploadSpeed = s.UploadSpeed,
                    Price =s.Price
                };
            }
        }

        public int Id { get; set; }

        [DisplayName("Plan Name: ")]
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string  SubscriptionName { get; set; }

        [DisplayName("Download speed: ")]
        [Required]
        public int DownloadSpeed { get; set; }

        [DisplayName("Upload speed: ")]
        [Required]
        public int UploadSpeed { get; set; }

        [DisplayName("Price per month: ")]
        [Required]
        public decimal Price { get; set; }

        [DisplayName("Description: ")]
        [DataType(DataType.MultilineText)]
        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}