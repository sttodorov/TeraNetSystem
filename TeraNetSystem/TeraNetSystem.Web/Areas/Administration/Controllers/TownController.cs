using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Web.Controllers;
using TeraNetSystem.Web.Areas.Administration.Models;
using TeraNetSystem.Models;
using System.Net;

namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    public class TownController : BaseController
    {

        // GET: Administration/Town
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListTowns()
        {
            var towns = this.Data.Towns.All().Select(TownViewModel.FromTown).ToList();
            return View(towns);
        }

        [HttpGet]
        public ActionResult CreatePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string newTownName)
        {
            if (ModelState.IsValid)
            {
                var newTown = new Town()
                {
                    TownName = newTownName
                };

                this.Data.Towns.Add(newTown);
                this.Data.SaveChanges();
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
        public ActionResult EditPage(TownViewModel edditedTown)
        {
            var townToBeEdiited = this.Data.Towns.All().FirstOrDefault(t => t.Id == edditedTown.Id);

            townToBeEdiited.TownName = edditedTown.TownName;
            this.Data.SaveChanges();

            return RedirectToAction("ListTowns");           
        }

    }
}