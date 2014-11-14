using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;
using TeraNetSystem.Web.Models;

namespace TeraNetSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ITeraNetData data)
            :base(data)
        {

        }

        public ActionResult Index()
        {
            var latestNews = this.GetLatestNews();
            return View(latestNews);
        }

        private IQueryable<NewsViewModel> GetLatestNews()
        {
            var result = this.Data.News.All()
                            .Select(NewsViewModel.FromNews)
                            .OrderByDescending(n => n.DateCreated);

            return result;
        }

    }
}