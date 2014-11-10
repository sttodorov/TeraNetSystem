using CaptchaMvc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;
using TeraNetSystem.Models;
using TeraNetSystem.Web.Areas.Administration.Models;
using TeraNetSystem.Web.Models;

namespace TeraNetSystem.Web.Controllers
{
    public class ServicesController : BaseController
    {
        public ServicesController(ITeraNetData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult All()
        {
            var services = this.Data.Subscriptions.All().Select(SubscriptionViewModel.FromSubscription);

            return View(services);
        }

        [HttpGet]
        public ActionResult Request(int? id)
        {
            var towns = this.Data.Towns.All().Select(TownCreateModel.FromOffice).ToList();

            var serviceId = id.GetValueOrDefault(1);

            var selectedList = new List<SelectListItem>();

            foreach (var item in towns)
            {
                selectedList.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Name });
            }

            var createModel = new ServiceRequestModel() { Towns = selectedList, SubscriptionId = serviceId };
            return View(createModel);
        }

        [HttpPost, CaptchaVerify("Code is not valid")]
        [ValidateAntiForgeryToken]
        public ActionResult Request(ServiceRequestModel request)
        {
            
            if (ModelState.IsValid)
            {
                var createdRequest = new Request()
                {
                    TownId = request.TownId,
                    Address = request.Address,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    SubscriptionId = request.SubscriptionId,
                    PhoneNumber = request.Phone
                };

                this.Data.Requests.Add(createdRequest);
                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Request for {0} subscription plan has been successfully send! We will call you in few days!",
                                                    this.Data.Subscriptions.All().FirstOrDefault(s=>s.Id==createdRequest.SubscriptionId).SubscriptionName);                
            }
            else
            {
                var towns = this.Data.Towns.All().Select(TownCreateModel.FromOffice).ToList();

                var selectedList = new List<SelectListItem>();

                foreach (var item in towns)
                {
                    selectedList.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Name });
                }
                request.Towns = selectedList;
                request.RequstCaptcha = string.Empty;

                TempData["Error"] = "Wrong validation code! Please enter code again!";

                return View(request);
            }

            return RedirectToAction("All");
        }
    }
}