using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TeraNetSystem.Data;
using TeraNetSystem.Web.Areas.Office.Models;
using TeraNetSystem.Web.Controllers;
using TeraNetSystem.Web.Models;
using System.Net;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    [Authorize(Roles = "Admin, OfficeMan")]
    public class PaymentController : ClientPaymentController
    {
        private const int PageSize = 5;

        public PaymentController(ITeraNetData data)
            : base(data)
        {
        }


        [HttpGet]
        public ActionResult ListPayments(PaymentSearchModel search, int? page)
        {

            var searchedPayments = this.Data.Payment.All().Select(PaymentViewModel.FromPayment);
            int pageNumber = page.GetValueOrDefault(1);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if ((PaymentSearchModel)TempData["Search"] != search && TempData.Count != 0 && TempData["Search"] != null)
            {
                search = (PaymentSearchModel)TempData["Search"];
            }
            TempData["Search"] = search;

            if (!User.IsInRole("Admin"))
            {
                var currentUserId = this.User.Identity.GetUserId();
                var currentOfficeTown = this.Data.Users.All().FirstOrDefault(u => u.Id == currentUserId).Office.Town.TownName;
                searchedPayments = searchedPayments.Where(p => p.Office.TownName == currentOfficeTown);
            }

            if (search.FromDate != null)
            {
                searchedPayments = searchedPayments.Where(p => p.DateCreated >= search.FromDate);
            }
            if (search.ToDate != null)
            {
                var toDate = new DateTime(search.ToDate.Value.Year, search.ToDate.Value.Month, search.ToDate.Value.Day, 23, 59, 59);
                searchedPayments = searchedPayments.Where(p => p.DateCreated <= toDate);

            }
            if (search.PerMonth != 0)
            {
                searchedPayments = searchedPayments.Where(p => p.PerMonth == search.PerMonth);
            }
            if (search.TownName != null)
            {
                searchedPayments = searchedPayments.Where(u => u.Client.TownName.Contains(search.TownName));
            }
            if (search.Address != null)
            {
                searchedPayments = searchedPayments.Where(u => u.Client.Address.Contains(search.Address));
            }
            if (search.UserName != null)
            {
                searchedPayments = searchedPayments.Where(u => u.Client.UserName.Contains(search.UserName));
            }
            if (search.FullName != null)
            {
                searchedPayments = searchedPayments.Where(u => u.Client.FullName.Contains(search.FullName));
            }

            var searchedPaymentsPage = searchedPayments
                          .OrderByDescending(x => x.DateCreated)
                          .ThenBy(x => x.Client.UserName)
                          .Skip((pageNumber - 1) * PageSize)
                          .Take(PageSize)
                          .ToList();

            ViewBag.Pages = Math.Ceiling((double)searchedPayments.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(searchedPaymentsPage);
        }


        [HttpGet]
        public ActionResult ListUsers(UserSearchModel search, int? page)
        {

            var searchedUsers = this.Data.Users.All().Select(ApplicationUserViewModel.FromUser);
            int pageNumber = page.GetValueOrDefault(1);
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if ((UserSearchModel)TempData["Search"] != search && TempData.Count != 0 && TempData["Search"] != null)
            {
                search = (UserSearchModel)TempData["Search"];
            }
            TempData["Search"] = search;

            if (search.TownName != null)
            {
                searchedUsers = searchedUsers.Where(u => u.TownName.Contains(search.TownName));
            }
            if (search.Address != null)
            {
                searchedUsers = searchedUsers.Where(u => u.Address.Contains(search.Address));
            }
            if (search.UserName != null)
            {
                searchedUsers = searchedUsers.Where(u => u.UserName.Contains(search.UserName));
            }
            if (search.FullName != null)
            {
                searchedUsers = searchedUsers.Where(u => u.FullName.Contains(search.FullName));
            }

            var searchedUsersPage = searchedUsers
                          .OrderBy(x => x.TownName)
                          .ThenBy(x => x.FullName)
                          .Skip((pageNumber - 1) * PageSize)
                          .Take(PageSize)
                          .ToList();

            ViewBag.Pages = Math.Ceiling((double)searchedUsers.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(searchedUsersPage);
        }

        [HttpGet]
        public ActionResult CreatePayment(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var officeManId = this.User.Identity.GetUserId();

            var client = this.Data.Users.All().Select(ApplicationUserViewModel.FromUser).FirstOrDefault(u => u.Id.ToString() == id);
            var office = this.Data.Users.All().Select(ApplicationUserViewModel.FromUser).FirstOrDefault(u => u.Id.ToString() == officeManId).Office;

            var payment = new PaymentViewModel()
            {
                DateCreated = DateTime.Now,
                Client = client,
                Office = office
            };

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePayment(PaymentViewModel payment)
        {
            
            if (payment.Client.Id == null || payment.Office.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (payment.PerMonth == 0)
            {
                TempData["Error"] = "Please select month!";
                return RedirectToAction("CreatePayment", new { id = payment.Client.Id });
            }

            var client = this.Data.Users.All().FirstOrDefault(u => u.Id == payment.Client.Id);
            var office = this.Data.Offices.All().FirstOrDefault(o => o.Id.ToString() == payment.Office.Id);

            if (client == null || office == null)
            {
                TempData["Error"] = "No such client or office!";
                return RedirectToAction("ListUsers");
            }

            var paymentToAdd = new Payment()
            {
                DateCreated = DateTime.Now,
                ClientId = payment.Client.Id,
                OfficeId = payment.Office.Id,
                PerMonth = payment.PerMonth,
            };

            client.Payments.Add(paymentToAdd);
            office.Payments.Add(paymentToAdd);

            this.Data.SaveChanges();

            TempData["Success"] = "Payment made successfully";
            
            return RedirectToAction("ListUsers");
        }

        [HttpGet]
        public ActionResult SearchByUsername(string username)
        {
            if (Request.IsAjaxRequest())
            {
                var searchedUsers = this.Data.Users.All().Select(SimpleUserViewModel.FromUser).Where(u => u.UserName.Contains(username)).OrderBy(u => u.UserName).Take(3).ToList();
                //return Json(searchedUsers, JsonRequestBehavior.AllowGet);
                return PartialView(searchedUsers);
            }
            return null;
        }
    }
}