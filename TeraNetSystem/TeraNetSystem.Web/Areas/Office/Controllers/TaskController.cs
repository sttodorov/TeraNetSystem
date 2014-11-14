using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using TeraNetSystem.Data;
using TeraNetSystem.Models;
using TeraNetSystem.Web.Areas.Office.Models;
using TeraNetSystem.Web.Controllers;
using TeraNetSystem.Web.Models;

namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    public class TaskController : BaseController
    {
        private const int PageSize = 3;
        public TaskController(ITeraNetData data)
            :base(data)
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
        public ActionResult Create(TaskViewModel taskTOCreate)
        {
            if(ModelState.IsValid)
            {
                var selectedTown = this.Data.Towns.All().FirstOrDefault(t => t.TownName == taskTOCreate.TownName);
                if(selectedTown == null)
                {
                    TempData["Error"] = String.Format("No {0} town found!",selectedTown.TownName);
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
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);                
            }

            var selectedTask = this.Data.Tasks.All().FirstOrDefault(t =>t.Id.ToString() == id);
            if(selectedTask==null)
            {

            }

            selectedTask.Compleated = true;
            selectedTask.DateCompleated = DateTime.Now;

            this.Data.SaveChanges();

            TempData["Success"] =  String.Format("Task with id - {0} mark as DONE sucessfully!", id);

            return RedirectToAction("ListTasks");

        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedTask = this.Data.Tasks.All().Select(TaskViewModel.FromTask).FirstOrDefault(t => t.Id.ToString() == id);

            if (selectedTask == null)
            {
                return HttpNotFound();
            }

            var networkersForCurrentTaskTown = this.Data.Users.All().Where(u => u.Town.TownName == selectedTask.TownName).ToList();
            
            var networkersList = new List<SelectListItem>();

            foreach (var worker in networkersForCurrentTaskTown)
            {
                networkersList.Add(new SelectListItem() { Value = worker.Id, Text = string.Format("{0} - {1}", worker.UserName, worker.FirstName) });
            }

            selectedTask.Netwrokers = networkersList;

            return View(selectedTask);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(TaskViewModel edittedTask)
        {
            if(edittedTask == null)
            {

            }

            var taskToEdit = this.Data.Tasks.All().FirstOrDefault(t => t.Id.ToString() == edittedTask.Id);

            taskToEdit.Address = edittedTask.Address;
            taskToEdit.Description = edittedTask.Description;
            taskToEdit.NetworkManId = edittedTask.NetworkerId;
            this.Data.SaveChanges();

            return RedirectToAction("ListTasks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var taskToBeDeleted = this.Data.Tasks.All().FirstOrDefault(r => r.Id.ToString() == id);

            if(taskToBeDeleted == null)
            {

            }

            this.Data.Tasks.Delete(taskToBeDeleted);
            this.Data.SaveChanges();

            TempData["Success"] = String.Format("Task with id - {0}  has been deleted successully!", id);

            return RedirectToAction("ListTasks");
        }
    }
}