namespace TeraNetSystem.Web.Areas.Office.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using TeraNetSystem.Models;

    public class PaymentSearchModel : UserSearchModel
    {
        //public int? Id { get; set; }

        [DisplayName("From: ")]
        public DateTime? FromDate { get; set; }

        [DisplayName("To: ")]
        public DateTime? ToDate { get; set; }

        [DisplayName("Per month: ")]
        public Month PerMonth { get; set; }

    }
}