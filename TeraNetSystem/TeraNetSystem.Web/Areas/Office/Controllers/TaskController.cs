using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Models;
using TeraNetSystem.Web.Areas.Office.Models;
using TeraNetSystem.Web.Controllers;

namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    public class TaskController : BaseController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskCreateViewModel taskTOCreate)
        {
            if(ModelState.IsValid)
            {
                var selectedTown = this.Data.Towns.All().FirstOrDefault(t => t.TownName == taskTOCreate.TownName);

                var task = new WorkTask
                {
                    Town = selectedTown,
                    Address = taskTOCreate.Address,
                    Description = taskTOCreate.Description,
                    NetworkManId = taskTOCreate.NetworkerId
                };

                this.Data.Tasks.Add(task);
                this.Data.SaveChanges();
            }
            else
            {
                return View(taskTOCreate);
            }


            return RedirectToAction("ListTasks");
        }
    }
}