namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using TeraNetSystem.Data;
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Areas.Administration.Models;
    using TeraNetSystem.Web.Controllers;

    public class TownController : AdministrationController
    {
        private const int PageSize = 3;

        public TownController(ITeraNetData data)
            : base(data)
        {

        }

        private bool IsUnique(string townName)
        {
            return this.Data.Towns.All().Any(t => t.TownName == townName);
        }

        [ChildActionOnly]
        private ActionResult GetSelectedTown(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedTown = this.Data.Towns.All().Select(TownViewModel.FromTown).FirstOrDefault(t => t.Id == id);

            if (selectedTown == null)
            {
                TempData["Error"] = "Town with ID {0} NOT FOUND";
                return RedirectToAction("ListTowns");
            }

            return View(selectedTown);
        }

        [HttpGet]
        public ActionResult ListTowns(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var allTowns = this.Data.Towns.All();

            var pageTowns = allTowns.OrderBy(x => x.Id)
                             .Skip((pageNumber - 1) * PageSize)
                             .Take(PageSize)
                             .Select(TownViewModel.FromTown)
                             .ToList();

            ViewBag.Pages = Math.Ceiling((double)allTowns.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(pageTowns);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TownCreateModel createdTown)
        {
            if (ModelState.IsValid)
            {
                bool hasSameTown = this.IsUnique(createdTown.Name);
                if(hasSameTown)
                {
                    TempData["Error"] = String.Format("Town {0} aslready exists!", createdTown.Name);
                    return View();
                }

                var newTown = new Town()
                {
                    TownName = createdTown.Name
                };

                this.Data.Towns.Add(newTown);
                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Town {0} added successfully!", createdTown.Name);
            }

            return RedirectToAction("ListTowns");
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            return this.GetSelectedTown(id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var townToBeDeleted = this.Data.Towns.All().FirstOrDefault(t => t.Id == id);

            foreach (var office in townToBeDeleted.Offices)
            {
                this.Data.Offices.Delete(office);
            }

            this.Data.Towns.Delete(townToBeDeleted);
            this.Data.SaveChanges();

            return RedirectToAction("ListTowns");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return this.GetSelectedTown(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TownViewModel edditedTown)
        {
            var townToBeEdiited = this.Data.Towns.GetById(edditedTown.Id);

            bool hasSameTown = this.IsUnique(edditedTown.TownName);
            if (hasSameTown)
            {
                TempData["Error"] = String.Format("Town {0} aslready exists!", edditedTown.TownName);
                return View(edditedTown);
            }

            townToBeEdiited.TownName = edditedTown.TownName;
            this.Data.SaveChanges();

            return RedirectToAction("ListTowns");
        }

    }
}