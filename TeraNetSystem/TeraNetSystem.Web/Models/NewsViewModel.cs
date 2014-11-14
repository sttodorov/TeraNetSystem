using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Models
{
    public class NewsViewModel
    {
         public static Expression<Func<News, NewsViewModel>> FromNews
        {
            get
            {
                return n => new NewsViewModel
                {
                    Id = n.Id,
                    DateCreated = n.DateCreated,
                    Title = n.Title,
                    Content = n.Content,
                    ImagePath = n.ImagePath,
                    Author = new ApplicationUserViewModel
                    {
                        Id = n.Author.Id,
                        UserName =  n.Author.UserName,
                        FullName = n.Author.FirstName
                    }
                };
            }
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength=5)]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string Content { get; set; }

        public string ImagePath { get; set; }

        public ApplicationUserViewModel Author { get; set; }
    }
}