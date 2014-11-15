namespace TeraNetSystem.Web.Areas.Office.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using TeraNetSystem.Data;

    [Authorize(Roles = "Admin,OfficeMan")]
    public abstract class OfficeController : NetworkController
    {
        public OfficeController(ITeraNetData data)
            : base(data)
        {

        }
    }
}