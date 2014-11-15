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

    public class TaskViewModel
    {
        public static Expression<Func<Request, TaskViewModel>> FromRequest
        {
            get
            {
                return r => new TaskViewModel
                {
                    Id = r.Id.ToString(),
                    TownName = r.Town.TownName,
                    Address = r.Address
                    
                };
            }
        }
        public static Expression<Func<WorkTask, TaskViewModel>> FromTask
        {
            get
            {
                return r => new TaskViewModel
                {
                    Id = r.Id.ToString(),
                    TownName = r.Town.TownName,
                    Address = r.Address,
                    Networker = r.NetworkMan,
                    Description = r.Description,

                };
            }
        }
        
        public string Id { get; set; }

        [DisplayName("Networkman: ")]
        [Required]
        public string NetworkerId { get; set; }

        public ApplicationUser Networker { get; set; }

        [DisplayName("Town: ")]
        [Required]
        public string TownName { get; set; }

        [DisplayName("Address: ")]
        [DataType(DataType.MultilineText)]
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Address { get; set; }

        [DisplayName("Description: ")]
        [DataType(DataType.MultilineText)]
        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Description { get; set; }

        public List<SelectListItem> Netwrokers { get; set; }
    }
}