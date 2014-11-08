namespace TeraNetSystem.Web.Areas.Administration.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using TeraNetSystem.Models;

    public class TownCreateModel
    {
        public static Expression<Func<Town, TownCreateModel>> FromOffice
        {
            get
            {
                return o => new TownCreateModel
                {
                    Id = o.Id,
                    Name = o.TownName
                };
            }
        }
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [DisplayName("Town name:")]
        public string Name { get; set; }

    }
}