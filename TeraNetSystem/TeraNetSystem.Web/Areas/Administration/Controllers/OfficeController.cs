namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using TeraNetSystem.Data;
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Areas.Administration.Models;
    using TeraNetSystem.Web.Controllers;

    public class OfficeController : BaseController
    {
        private const int PageSize = 5;

        public OfficeController(ITeraNetData data)
            : base(data)
        {

        }

        [HttpGet]
        public ActionResult ListOffices(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var allOffices = this.Data.Offices.All();

            var pageOffices = allOffices.OrderBy(x => x.Id)
                            .Skip((pageNumber - 1) * PageSize)
                            .Take(PageSize)
                            .Select(AdminOfficeViewModel.FromOffice)
                            .ToList();

            ViewBag.Pages = Math.Ceiling((double)allOffices.Count() / PageSize);
            return View(pageOffices);
        }

        [HttpGet]
        public ActionResult CreatePage()
        {
            var towns = this.Data.Towns.All().Select(TownCreateModel.FromOffice).ToList();

            var selectedList = new List<SelectListItem>();

            foreach (var item in towns)
            {
                selectedList.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Name });
            }

            var createModel = new OfficeCreateModel() { Towns = selectedList };
            return View(createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OfficeCreateModel newOfficeModel)
        {
            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                string imageExt = string.Empty;

                if (newOfficeModel.Image == null)
                {
                    imagePath = "~/Content/images/office.png";
                }
                else
                {
                    imagePath = "~/Files/offices/" + newOfficeModel.TownId;
                    imageExt = Path.GetExtension(newOfficeModel.Image.FileName);
                    string imageName = Guid.NewGuid().ToString();
                    if (!Directory.Exists(Server.MapPath(imagePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(imagePath));
                    }
                    imagePath += "/" + imageName + imageExt;
                    newOfficeModel.Image.SaveAs(Server.MapPath(imagePath));
                }

                var newOffice = new Office()
                {
                    TownId = newOfficeModel.TownId,
                    Address = newOfficeModel.Address,
                    Phone = newOfficeModel.Phone,
                    ImagePath = imagePath

                };

                this.Data.Offices.Add(newOffice);
                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Office in {0} added successfully!", newOffice.Town.TownName);

            }

            return RedirectToAction("ListOffices");
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedOffice = this.Data.Offices.All().Select(AdminOfficeViewModel.FromOffice).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedOffice == null)
            {
                return HttpNotFound();
            }

            return View(selectedOffice);
        }

        [HttpGet]
        public ActionResult EditPage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedOffice = this.Data.Offices.All().Select(AdminOfficeViewModel.FromOffice).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedOffice == null)
            {
                return HttpNotFound();
            }

            return View(selectedOffice);
        }

        [HttpPost, ActionName("EditPage")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPagePost(AdminOfficeViewModel edditedOffice)
        {
            var officeToEdit = this.Data.Offices.All().FirstOrDefault(t => t.Id.ToString() == edditedOffice.Id);

            officeToEdit.Address = edditedOffice.Address;
            officeToEdit.Phone = edditedOffice.Phone;
            this.Data.SaveChanges();

            return RedirectToAction("ListOffices");
        }

        [HttpGet]
        public ActionResult DeletePage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedOffice = this.Data.Offices.All().Select(AdminOfficeViewModel.FromOffice).FirstOrDefault(t => t.Id == id);

            if (selectedOffice == null)
            {
                return HttpNotFound();
            }

            return View(selectedOffice);
        }

        [HttpPost, ActionName("DeletePage")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePagePost(string id)
        {
            var officeToBeDeleted = this.Data.Offices.All().FirstOrDefault(t => t.Id.ToString() == id);

            foreach (var payment in officeToBeDeleted.Payments)
            {
                this.Data.Payment.Delete(payment);
            }



            this.Data.Offices.Delete(officeToBeDeleted);
            this.Data.SaveChanges();

            return RedirectToAction("ListOffices");
        }
    }
}