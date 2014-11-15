using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;
using TeraNetSystem.Web.Controllers;

namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    [Authorize(Roles="NetworkMan,OfficeMan,Admin")]
    public class NetworkController : BaseController
    {
        public NetworkController(ITeraNetData data)
            : base(data)
        {

        }

        [OutputCache(Duration = 60 * 60 * 2, VaryByParam = "townName")]
        protected List<SelectListItem> GetNetworkers(string townName)
        {
            var networkersForCurrentTaskTown = this.Data.Users.All().Where(u => u.Town.TownName == townName);
            var networkersList = new List<SelectListItem>();

            foreach (var worker in networkersForCurrentTaskTown)
            {

                networkersList.Add(new SelectListItem()
                {
                    Value = worker.Id,
                    Text = string.Format("{0} - {1}", worker.UserName, worker.FirstName)
                });
            }
            return networkersList;
        }
    }
}