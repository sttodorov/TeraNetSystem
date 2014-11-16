using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeraNetSystem.Web.Areas.Office.Models
{
    public class UserSearchModel
    {

        [DisplayName("Town: ")]
        public string TownName { get; set; }

        [DisplayName("Username: ")]
        [UIHint("StringNoAutocomplete")]
        public string UserName { get; set; }

        [DisplayName("Full name: ")]
        public string FullName { get; set; }

        [DisplayName("Address: ")]
        public string Address { get; set; }

    }
}