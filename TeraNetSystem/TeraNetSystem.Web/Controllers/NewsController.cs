using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;
using TeraNetSystem.Web.Models;

namespace TeraNetSystem.Web.Controllers
{
    public class NewsController : BaseController
    {
        public NewsController(ITeraNetData data)
            :base(data)
        {
        }

        [ChildActionOnly]
        protected ActionResult GetSelectedNews(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedNews = this.Data.News.All().Select(NewsViewModel.FromNews).FirstOrDefault(n => n.Id == id);

            if (selectedNews == null)
            {
                TempData["Error"] = string.Format("News with Id {0} NOT FOUND!", id);
                return RedirectToAction("ListNews");
            }

            return View(selectedNews);
        }
 
        [HttpGet]
        public ActionResult NewsDetails(int? id)
        {
            return GetSelectedNews(id);
        }
    }
}