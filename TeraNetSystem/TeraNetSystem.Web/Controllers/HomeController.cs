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
        [HttpGet]
        public ActionResult Careers()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TermsOfUse()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var latestNews = this.GetLatestNews();
            return View(latestNews);
        }

        [OutputCache(Duration=60*10)]
        private IQueryable<NewsViewModel> GetLatestNews()
        {
            var result = this.Data.News.All()
                            .Select(NewsViewModel.FromNews)
                            .OrderByDescending(n => n.DateCreated);

            return result;
        }

    }
}