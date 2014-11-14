using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeraNetSystem.Web.Areas.Administration.Models
{
    public class NewsCreateModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Content { get; set; }

        
        [DisplayName("Image:")]
        public HttpPostedFileBase Image { get; set; }
    }
}