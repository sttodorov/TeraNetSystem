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
    using TeraNetSystem.Web.Models;
    using TeraNetSystem.Web.Areas.Administration.Models;

    public class OfficeController : AdministrationController
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

            var pageOffices = allOffices.OrderBy(x => x.Town.TownName)
                            .Skip((pageNumber - 1) * PageSize)
                            .Take(PageSize)
                            .Select(OfficeViewModel.FromOffice)
                            .ToList();

            ViewBag.Pages = Math.Ceiling((double)allOffices.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

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
                    Name= newOfficeModel.Name,
                    Address = newOfficeModel.Address,
                    Phone = newOfficeModel.Phone,
                    ImagePath = imagePath

                };

                this.Data.Offices.Add(newOffice);
                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Office {0} added successfully!", newOffice.Name);

            }

            return RedirectToAction("ListOffices");
        }

        [ChildActionOnly]
        private ActionResult GetSelectedOffice(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedOffice = this.Data.Offices.All().Select(AdminOfficeViewModel.FromOffice).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedOffice == null)
            {
                TempData["Error"] = "Office with ID {0} NOT FOUND";
                return RedirectToAction("ListOffices");
            }

            return View(selectedOffice);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            return this.GetSelectedOffice(id);   
        }

        [HttpGet]
        public ActionResult EditPage(string id)
        {
            return this.GetSelectedOffice(id);   
        }

        [HttpPost, ActionName("EditPage")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPagePost(AdminOfficeViewModel editedOffice)
        {
            var officeToEdit = this.Data.Offices.GetById(new Guid(editedOffice.Id));

            if (ModelState.IsValid)
            {

                officeToEdit.Name = editedOffice.Name;
                officeToEdit.Address = editedOffice.Address;
                officeToEdit.Phone = editedOffice.Phone;

                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Office {0} updated successfully!", editedOffice.Name);
                return RedirectToAction("ListOffices");
            }
            else
            {
                editedOffice.ImagePath = officeToEdit.ImagePath;
                return View(editedOffice);
            }
        }

        [HttpGet]
        public ActionResult DeletePage(string id)
        {
            return this.GetSelectedOffice(id);
        }

        [HttpPost, ActionName("DeletePage")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePagePost(string id)
        {
            var officeToBeDeleted = this.Data.Offices.GetById(new Guid(id));


            this.Data.Offices.Delete(officeToBeDeleted);
            this.Data.SaveChanges();

            TempData["Success"] = String.Format("Office {0} deleted successfully!", officeToBeDeleted.Name);

            return RedirectToAction("ListOffices");
        }

    }
}