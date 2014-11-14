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
            :base(data)
        {

        }

        [HttpGet]
        public ActionResult AllOffices()
        {
            var offices = this.Data.Offices.All().Select(OfficeViewModel.FromOffice);

            return View(offices);
        }
    }
}