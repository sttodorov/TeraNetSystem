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

    public class TownController : BaseController
    {
        private const int PageSize = 3;

        public TownController(ITeraNetData data)
            : base(data)
        {

        }
        // GET: Administration/Town
        public ActionResult Index()
        {
            return View();
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
        public ActionResult CreatePage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TownCreateModel createdTown)
        {
            if (ModelState.IsValid)
            {
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
        public ActionResult DeletePage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedTown = this.Data.Towns.All().Select(TownViewModel.FromTown).FirstOrDefault(t => t.Id == id);

            if (selectedTown == null)
            {
                return HttpNotFound();
            }

            return View(selectedTown);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePage(int id)
        {
            var townToBeDeleted = this.Data.Towns.All().FirstOrDefault(t => t.Id == id);

            foreach (var office in townToBeDeleted.Offices)
            {
                this.Data.Offices.Delete(office);
            }
            foreach (var client in townToBeDeleted.Users)
            {
                this.Data.Users.Delete(client);
            }

            this.Data.Towns.Delete(townToBeDeleted);
            this.Data.SaveChanges();

            return RedirectToAction("ListTowns");
        }

        [HttpGet]
        public ActionResult EditPage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedTown = this.Data.Towns.All().Select(TownViewModel.FromTown).FirstOrDefault(t => t.Id == id);

            if (selectedTown == null)
            {
                return HttpNotFound();
            }

            return View(selectedTown);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPage(TownViewModel edditedTown)
        {
            var townToBeEdiited = this.Data.Towns.All().FirstOrDefault(t => t.Id == edditedTown.Id);

            townToBeEdiited.TownName = edditedTown.TownName;
            this.Data.SaveChanges();

            return RedirectToAction("ListTowns");
        }

    }
}