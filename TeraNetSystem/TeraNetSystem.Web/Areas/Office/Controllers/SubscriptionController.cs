using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;
using TeraNetSystem.Web.Controllers;
using TeraNetSystem.Web.Areas.Office.Models;
using System.Net;
using TeraNetSystem.Web.Models;
using TeraNetSystem.Models;

namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    public class SubscriptionController : BaseController
    {
        private const int PageSize = 5;
        public SubscriptionController(ITeraNetData data)
            : base(data)
        {

        }

        [HttpGet]
        public ActionResult ListSubscriptions(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var allSubscriptions = this.Data.Subscriptions.All();
            var pageSubscriptions = allSubscriptions
                            .Skip((pageNumber - 1) * PageSize)
                            .Take(PageSize)
                            .Select(OfficeSubscriptionViewModel.FromSubscription);

            ViewBag.Pages = Math.Ceiling((double)allSubscriptions.Count() / PageSize);
            return View(pageSubscriptions);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedSubscription = this.Data.Subscriptions.All()
                                        .Select(OfficeSubscriptionViewModel.FromSubscription)
                                        .FirstOrDefault(a => a.Id == id);

            if (selectedSubscription == null)
            {
                return HttpNotFound();
            }

            return View(selectedSubscription);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedSubscription = this.Data.Subscriptions.All()
                                        .Select(OfficeSubscriptionViewModel.FromSubscription)
                                        .FirstOrDefault(a => a.Id == id);

            if (selectedSubscription == null)
            {
                return HttpNotFound();
            }

            return View(selectedSubscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubscriptionViewModel edittedSubscription)
        {
            var selectedSubscription = this.Data.Subscriptions.All().FirstOrDefault(s => s.Id == edittedSubscription.Id);

            selectedSubscription.Price = edittedSubscription.Price;
            selectedSubscription.DownloadSpeed = edittedSubscription.DownloadSpeed;
            selectedSubscription.UploadSpeed = edittedSubscription.UploadSpeed;
            selectedSubscription.Description = edittedSubscription.Description;

            this.Data.SaveChanges();

            return RedirectToAction("ListSubscriptions");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedSubscription = this.Data.Subscriptions.All().Select(SubscriptionViewModel.FromSubscription).FirstOrDefault(t => t.Id == id);

            if (selectedSubscription == null)
            {
                return HttpNotFound();
            }

            return View(selectedSubscription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int? id)
        {
            var subscriptionToBeDeleted = this.Data.Subscriptions.All().FirstOrDefault(t => t.Id == id);

            this.Data.Subscriptions.Delete(subscriptionToBeDeleted);
            this.Data.SaveChanges();

            TempData["Success"] = String.Format("Subscription plan \"{0}\" deleted successfully!", subscriptionToBeDeleted.SubscriptionName);

            return RedirectToAction("ListSubscriptions");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SubscriptionViewModel subscription)
        {
            if(ModelState.IsValid)
            {
                var subscriptionToBeCreated = new Subscription()
                {
                    Id = subscription.Id,
                    SubscriptionName =  subscription.SubscriptionName,
                    Price = subscription.Price,
                    DownloadSpeed = subscription.DownloadSpeed,
                    UploadSpeed = subscription.UploadSpeed,
                    Description = subscription.Description
                };

                this.Data.Subscriptions.Add(subscriptionToBeCreated);
                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Subscription plan \"{0}\" created successfully!", subscriptionToBeCreated.SubscriptionName);
            }

            return RedirectToAction("ListSubscriptions");
        }
    }
}