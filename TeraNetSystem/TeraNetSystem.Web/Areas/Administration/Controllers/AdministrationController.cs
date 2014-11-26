namespace TeraNetSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections;
    using System.Web.Mvc;

    using TeraNetSystem.Data;
    using TeraNetSystem.Web.Controllers;

    [Authorize(Roles="Admin")]
    public abstract class AdministrationController : BaseController
    {
        public AdministrationController(ITeraNetData data)
            :base(data)
        {
        }

    }
}