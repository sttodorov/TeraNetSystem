namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using TeraNetSystem.Web.Controllers;
    using TeraNetSystem.Web.Areas.Office.Models;
using TeraNetSystem.Data;

    public class RequestController : BaseController
    {
        private const int PageSize = 5;

        public RequestController(ITeraNetData data)
            :base(data)
        {
        }

        [HttpGet]
        public ActionResult ListRequests(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var currentUserId = this.User.Identity.GetUserId();
            var currentOfficeTownId = this.Data.Users.All().FirstOrDefault(u => u.Id == currentUserId).TownId;

            var requestForCurrentTown = this.Data.Requests.All().Where(r => r.TownId == currentOfficeTownId);

            var requestsPage = requestForCurrentTown.OrderBy(x => x.Id)
                             .Skip((pageNumber - 1) * PageSize)
                             .Take(PageSize)
                             .Select(RequestViewModel.FromRequest)
                             .ToList();

            ViewBag.Pages = Math.Ceiling((double)requestForCurrentTown.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(requestsPage);
        }

    }
}