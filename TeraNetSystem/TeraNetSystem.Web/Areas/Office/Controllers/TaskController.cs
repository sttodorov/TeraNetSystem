namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using TeraNetSystem.Data;
    using TeraNetSystem.Models;
    using TeraNetSystem.Web.Areas.Office.Models;
    using TeraNetSystem.Web.Controllers;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Web.Security;

    [Authorize(Roles = "Admin,OfficeMan,NetworkMan")]
    public class TaskController : NetworkController
    {
        private const int PageSize = 3;

        public TaskController(ITeraNetData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult ListTasks(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);

            var allTasks = this.Data.Tasks.All().Where(t => t.Compleated == false);
            var currentUserId = this.User.Identity.GetUserId();

            if (this.User.IsInRole("NetworkMan"))
            {
                allTasks = allTasks.Where(t => t.NetworkManId == currentUserId);
            }

            var requestsPage = allTasks.OrderBy(x => x.Id)
                           .Skip((pageNumber - 1) * PageSize)
                           .Take(PageSize)
                           .Select(TaskViewModel.FromTask)
                           .ToList();

            ViewBag.Pages = Math.Ceiling((double)allTasks.Count() / PageSize);
            ViewBag.PageNumber = pageNumber;

            return View(requestsPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,OfficeMan")]
        public ActionResult Create(TaskViewModel taskTOCreate)
        {
            if (ModelState.IsValid)
            {
                var selectedTown = this.Data.Towns.All().FirstOrDefault(t => t.TownName == taskTOCreate.TownName);
                if (selectedTown == null)
                {
                    TempData["Error"] = String.Format("No {0} town found!", selectedTown.TownName);
                    return View(taskTOCreate);
                }
                var task = new WorkTask
                {
                    TownId = selectedTown.Id,
                    Address = taskTOCreate.Address,
                    Description = taskTOCreate.Description,
                    NetworkManId = taskTOCreate.NetworkerId
                };

                this.Data.Tasks.Add(task);
                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Request for town: {0} address: {1} made successfully!", task.Town.TownName, task.Address);
            }
            else
            {
                TempData["Error"] = "Your data is wrong";
                return View(taskTOCreate);
            }

            return RedirectToAction("ListTasks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAsDone(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedTask = this.Data.Tasks.GetById(new Guid(id));

            if (selectedTask == null)
            {
                TempData["Error"] = String.Format("Task with ID {0} Not Found!", id);
            }
            else
            {

                selectedTask.Compleated = true;
                selectedTask.DateCompleated = DateTime.Now;

                this.Data.SaveChanges();

                TempData["Success"] = String.Format("Task with id - {0} mark as DONE sucessfully!", id);
            }

            return RedirectToAction("ListTasks");

        }

        [HttpGet]
        [Authorize(Roles = "Admin,OfficeMan")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedTask = this.Data.Tasks.All().Select(TaskViewModel.FromTask).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedTask == null)
            {
                TempData["Error"] = String.Format("Task with ID {0} Not Found!", id);
            }

            var networkersList = this.GetNetworkers(selectedTask.TownName);

            selectedTask.Netwrokers = networkersList;
            selectedTask.Netwrokers.FirstOrDefault(w => w.Value == selectedTask.Networker.Id).Selected = true;

            return View(selectedTask);
        }

        

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,OfficeMan")]
        public ActionResult EditPost(TaskViewModel editedTask)
        {
            if (ModelState.IsValid)
            {
                var taskToEdit = this.Data.Tasks.GetById(new Guid(editedTask.Id));

                taskToEdit.Address = editedTask.Address;
                taskToEdit.Description = editedTask.Description;
                taskToEdit.NetworkManId = editedTask.NetworkerId;
                this.Data.SaveChanges();

                return RedirectToAction("ListTasks");
            }
            else
            {
                editedTask.Netwrokers = this.GetNetworkers(editedTask.TownName);
                editedTask.Netwrokers.FirstOrDefault(w => w.Value == editedTask.Networker.Id).Selected = true;
                return View(editedTask);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,OfficeMan")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var taskToBeDeleted = this.Data.Tasks.GetById(new Guid(id));

            if (taskToBeDeleted == null)
            {
                TempData["Error"] = String.Format("Task with ID {0} Not Found!", id);
            }

            this.Data.Tasks.Delete(taskToBeDeleted);
            this.Data.SaveChanges();

            TempData["Success"] = String.Format("Task with id - {0}  has been deleted successully!", id);

            return RedirectToAction("ListTasks");
        }
    }
}