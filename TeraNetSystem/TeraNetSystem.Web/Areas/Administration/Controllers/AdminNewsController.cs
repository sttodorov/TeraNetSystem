using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TeraNetSystem.Data;
using TeraNetSystem.Models;
using TeraNetSystem.Web.Controllers;
using TeraNetSystem.Web.Models;
using System.IO;
using TeraNetSystem.Web.Areas.Administration.Models;

namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminNewsController : NewsController
    {
        private const int PageSize = 3;
        public AdminNewsController(ITeraNetData data)
            : base(data)
        {
        }

        private IQueryable<NewsViewModel> GetNews(int page, int? count = null)
        {
            int newsToTake = count != null ? count.GetValueOrDefault(1) : PageSize;

            var allNews = this.Data.News.All();
            var news = allNews
                       .Select(NewsViewModel.FromNews)
                       .OrderByDescending(n => n.DateCreated)
                       .Skip((page - 1) * PageSize)
                       .Take(newsToTake);


            ViewBag.Pages = Math.Ceiling((double)allNews.Count() / PageSize);
            ViewBag.PageNumber = page;

            return news;
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult ListNews(int? page)
        {
            var pageNumber = page.GetValueOrDefault(1);
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            return View(GetNews(pageNumber));
        }



        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return GetSelectedNews(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsViewModel editedNews)
        {
            if (editedNews.Title != null && editedNews.Content != null)
            {
                var newsToEdit = this.Data.News.All().FirstOrDefault(n => n.Id == editedNews.Id);
                newsToEdit.Title = editedNews.Title;
                newsToEdit.Content = editedNews.Content;

                this.Data.SaveChanges();
                TempData["Success"] = "News editted successfully";
                return RedirectToAction("ListNews");
            }
            else
            {
                TempData["Error"] = "Please provide correct information!";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var selectedNews = this.Data.News.All().FirstOrDefault(n => n.Id == id);

            if (selectedNews == null)
            {
                TempData["Erorr"] = String.Format("News woth ID {0} NOT FOUND!", id);
            }
            else
            {
                this.Data.News.Delete(selectedNews);
                this.Data.SaveChanges();
                TempData["Success"] = "News deleted successfully";
            }

            return RedirectToAction("ListNews");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsCreateModel newsToCreate)
        {

            if (ModelState.IsValid)
            {
                var currentUserId = this.User.Identity.GetUserId();

                string imagePath = string.Empty;
                string imageExt = string.Empty;

                if (newsToCreate.Image == null)
                {
                    imagePath = "~/Content/images/news.png";
                }
                else
                {
                    string imageName = Guid.NewGuid().ToString();
                    imageExt = Path.GetExtension(newsToCreate.Image.FileName);
                    
                    imagePath = "~/Files/news/" +  imageName + imageExt;
                    newsToCreate.Image.SaveAs(Server.MapPath(imagePath));
                }

                var newNews = new News()
                {
                    Title = newsToCreate.Title,
                    Content = newsToCreate.Content,
                    AuthorId = currentUserId,
                    ImagePath = imagePath
                };

                this.Data.News.Add(newNews);
                this.Data.SaveChanges();
                TempData["Success"] = "News added successfully!!";

            }
            else
            {
                TempData["Error"] = "Please provide correct information!";
            }

            return RedirectToAction("ListNews");
        }
    }
}