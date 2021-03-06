﻿namespace TeraNetSystem.Web.Areas.Office.Controllers
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
    using System.Net;

    public class RequestController : OfficeController
    {
        private const int PageSize = 5;

        public RequestController(ITeraNetData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult ListRequests(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var currentUserId = this.User.Identity.GetUserId();
            var currentOfficeTownId = this.Data.Users.All().FirstOrDefault(u => u.Id == currentUserId).TownId;

            var requestForCurrentTown = this.Data.Requests.All().Where(r => r.Approved == false);
            if (User.IsInRole("OfficeMan"))
            {
                requestForCurrentTown = this.Data.Requests.All().Where(r => r.TownId == currentOfficeTownId);
            }

            var requestsPage = requestForCurrentTown.OrderBy(x => x.Id)
                             .Skip((pageNumber - 1) * PageSize)
                             .Take(PageSize)
                             .Select(RequestViewModel.FromRequest)
                             .ToList();

            ViewBag.Pages = Math.Ceiling((double)requestForCurrentTown.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(requestsPage);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedRequests = this.Data.Requests.All().Select(RequestViewModel.FromRequest).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedRequests == null)
            {
            }

            return View(selectedRequests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var requestToBeDeleted = this.Data.Requests.GetById(new Guid(id));

            this.Data.Requests.Delete(requestToBeDeleted);
            this.Data.SaveChanges();

            TempData["Success"] = String.Format("Request with id - {0}  has been deleted successully!", id);

            return RedirectToAction("ListRequests");
        }

        [HttpGet]
        public ActionResult Transfer(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedRequestsAsTask = this.Data.Requests.All().Select(TaskViewModel.FromRequest).FirstOrDefault(t => t.Id.ToString() == id);

            var networkersList = this.GetNetworkers(selectedRequestsAsTask.TownName);

            selectedRequestsAsTask.Netwrokers = networkersList;

            this.Data.Requests.All().FirstOrDefault(r => r.Id.ToString() == id).Approved = true;
            this.Data.SaveChanges();

            return View(selectedRequestsAsTask);
        }

    }
}