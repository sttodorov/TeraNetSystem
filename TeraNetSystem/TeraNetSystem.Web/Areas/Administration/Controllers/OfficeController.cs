using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Models;
using TeraNetSystem.Web.Areas.Administration.Models;
using TeraNetSystem.Web.Controllers;
using TeraNetSystem.Data;

namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    public class OfficeController : BaseController
    {
        private const int PageSize = 5;

        public OfficeController(ITeraNetData data)
            :base(data)
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
                            .Select(OfficeViewModel.FromOffice)
                            .ToList();

            ViewBag.Pages = Math.Ceiling((double)allOffices.Count() / PageSize);
            return View(pageOffices);
        }

        [HttpGet]
        public ActionResult CreatePage()
        {
            var towns = this.Data.Towns.All().ToList();

            ViewBag.Towns = towns;

            ViewBag.TownId = new SelectList(this.Data.Towns.All().ToList(), "Id", "TownName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(OfficeCreateModel newOfficeModel)
        {
            if (ModelState.IsValid)
            {
                string imagePath = string.Empty;
                string imageExt = Path.GetExtension(newOfficeModel.Image.FileName);

                if(newOfficeModel.Image == null)
                {
                    imagePath = "~/Content/images/office.png";
                }
                else
                {
                    imagePath ="~/Files/offices/" + newOfficeModel.TownId;
                    string imageName = Guid.NewGuid().ToString();
                    if(!Directory.Exists(Server.MapPath(imagePath)))
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

            var selectedOffice = this.Data.Offices.All().Select(OfficeViewModel.FromOffice).FirstOrDefault(t => t.Id.ToString() == id);

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

            var selectedOffice = this.Data.Offices.All().Select(OfficeViewModel.FromOffice).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedOffice == null)
            {
                return HttpNotFound();
            }

            return View(selectedOffice);
        }

        [HttpPost, ActionName("EditPage")]
        
        public ActionResult EditPagePost(OfficeViewModel edditedOffice)
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

            var selectedOffice = this.Data.Offices.All().Select(OfficeViewModel.FromOffice).FirstOrDefault(t => t.Id == id);

            if (selectedOffice == null)
            {
                return HttpNotFound();
            }

            return View(selectedOffice);
        }

        [HttpPost, ActionName("DeletePage")]
        public ActionResult DeletePagePost(string id)
        {
            var officeToBeDeleted = this.Data.Offices.All().FirstOrDefault(t => t.Id.ToString() == id);

            foreach (var payment in officeToBeDeleted.Payments)
            {
                this.Data.Payment.Delete(payment);
            }

            this.Data.Offices.Delete(officeToBeDeleted);
            this.Data.SaveChanges();

            return Redirect("ListOffices");
        }


    }
}