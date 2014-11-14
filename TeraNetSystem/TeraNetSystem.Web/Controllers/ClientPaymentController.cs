using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TeraNetSystem.Data;
using TeraNetSystem.Web.Models;
using System.Net;

namespace TeraNetSystem.Web.Controllers
{
    public class ClientPaymentController : BaseController
    {
        private const int PageSize = 5;

        public ClientPaymentController(ITeraNetData data)
            :base(data)
        {
        }

        [HttpGet]
        public ActionResult MyPayments(int? page)
        {
            var currentUserId = User.Identity.GetUserId();

            var searchedPayments = this.Data.Payment.All()
                                .Select(PaymentViewModel.FromPayment)
                                .Where(u => u.Client.Id == currentUserId);

            int pageNumber = page.GetValueOrDefault(1);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
           
            var searchedPaymentsPage = searchedPayments
                          .OrderByDescending(x => x.DateCreated)
                          .ThenByDescending(x=>x.PerMonth)
                          .Skip((pageNumber - 1) * PageSize)
                          .Take(PageSize)
                          .ToList();

            ViewBag.Pages = Math.Ceiling((double)searchedPayments.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(searchedPaymentsPage);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var searchedPayment = this.Data.Payment.All().Select(PaymentViewModel.FromPayment).FirstOrDefault(p => p.Id == id);
            if (searchedPayment == null)
            {
                TempData["Error"] = "No payment with such ID";
                return RedirectToAction("MyPayments");
            }

            return View(searchedPayment);
        }
    }
}