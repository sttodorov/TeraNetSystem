using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;
using TeraNetSystem.Web.Models;

namespace TeraNetSystem.Web.Controllers
{
    public class ContactController : BaseController
    {
        public ContactController(ITeraNetData data)
            : base(data)
        {

        }

        [HttpGet]
        public ActionResult AllOffices()
        {
            var offices = this.GetAllOfiices();
            return View(offices);
        }

        [OutputCache(Duration = 60 * 60 * 24)]
        private IQueryable<OfficeViewModel> GetAllOfiices()
        {
            return this.Data.Offices.All().Select(OfficeViewModel.FromOffice);
        }
    }
}