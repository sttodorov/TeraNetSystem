namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Net;
    using System.Linq;
    using System.Web.Mvc;

    using TeraNetSystem.Data;
    using TeraNetSystem.Web.Controllers;
    using TeraNetSystem.Web.Models;
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Areas.Administration.Models;

    public class SubscriptionController : AdministrationController
    {
        private const int PageSize = 5;

        public SubscriptionController(ITeraNetData data)
            : base(data)
        {

        }

        private bool HasSameRecord(string subscriptionName)
        {
            return this.Data.Subscriptions.All().Any(t => t.SubscriptionName == subscriptionName);
        }
        
        
        private ActionResult GetSelectedSubscription(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedSubscription = this.Data.Subscriptions.All().Select(OfficeSubscriptionViewModel.FromSubscription).FirstOrDefault(t => t.Id == id);

            if (selectedSubscription == null)
            {
                TempData["Error"] = "Subscriptions with ID {0} NOT FOUND";
                return RedirectToAction("ListSubscriptions");
            }

            return View(selectedSubscription);
        }


        [HttpGet]
        public ActionResult ListSubscriptions(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var allSubscriptions = this.Data.Subscriptions.All();
            var pageSubscriptions = allSubscriptions
                            .OrderBy(s => s.Id)
                            .Skip((pageNumber - 1) * PageSize)
                            .Take(PageSize)
                            .Select(OfficeSubscriptionViewModel.FromSubscription);

            ViewBag.Pages = Math.Ceiling((double)allSubscriptions.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(pageSubscriptions);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            return this.GetSelectedSubscription(id);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return this.GetSelectedSubscription(id);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return this.GetSelectedSubscription(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubscriptionViewModel edittedSubscription)
        {
            if (ModelState.IsValid)
            {
                var selectedSubscription = this.Data.Subscriptions.All().FirstOrDefault(s => s.Id == edittedSubscription.Id);

                selectedSubscription.Price = edittedSubscription.Price;
                selectedSubscription.DownloadSpeed = edittedSubscription.DownloadSpeed;
                selectedSubscription.UploadSpeed = edittedSubscription.UploadSpeed;
                selectedSubscription.Description = edittedSubscription.Description;

                this.Data.SaveChanges();

                TempData["Success"] = string.Format("Subscription plan {0} updeted successfully!", selectedSubscription.SubscriptionName);
                return RedirectToAction("ListSubscriptions");
            }
            else
            {
                return View(edittedSubscription);
            }
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
            if (ModelState.IsValid)
            {
                if(this.HasSameRecord(subscription.SubscriptionName))
                {
 

                    TempData["Error"] = String.Format("Subscription with name {0} aslready exists!", subscription.SubscriptionName);
                    return View();
                }

                var subscriptionToBeCreated = new Subscription()
                {
                    Id = subscription.Id,
                    SubscriptionName = subscription.SubscriptionName,
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