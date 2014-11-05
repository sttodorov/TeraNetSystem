namespace TeraNetSystem.Web.Areas.Administration.Models
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using TeraNetSystem.Models;

    public class TownViewModel
    {
        public static Expression<Func<Town, TownViewModel>> FromTown
        {
            get
            {
                return t => new TownViewModel
                {
                    Id = t.Id,
                    TownName = t.TownName,
                    OfficesCount = t.Offices.Count,
                    UsersCount = t.Users.Count
                };
            }
        }

        public int Id { get; set; }

        [DisplayName("Town name:")]
        public string TownName { get; set; }

        [DisplayName("Offices count:")]
        public int OfficesCount { get; set; }

        [DisplayName("Users count:")]
        public int UsersCount { get; set; }
    }
}