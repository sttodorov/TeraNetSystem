using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Areas.Office.Models
{
    public class SimpleUserViewModel
    {
        public static Expression<Func<ApplicationUser, SimpleUserViewModel>> FromUser
        {
            get
            {
                return u => new SimpleUserViewModel
                {
                    Id = u.Id.ToString(),
                    UserName = u.UserName,
                    FullName = u.FirstName + " " + u.LastName
                };
            }
        }

        public string Id { get; set; }

        [DisplayName("Username: ")]
        public string UserName { get; set; }

        [DisplayName("Full Name: ")]
        public string FullName { get; set; }
    }
}