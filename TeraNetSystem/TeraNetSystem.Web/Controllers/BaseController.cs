using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeraNetSystem.Data;

namespace TeraNetSystem.Web.Controllers
{
    public class BaseController : Controller
    {
        private ITeraNetData data;

        public BaseController()
        {
            
        }

        public BaseController(ITeraNetData data)
        {
            this.Data = data;
        }

        protected ITeraNetData Data
        {
            get
            {
                return this.data;
            }
            private set
            {
                this.data = value;
            }
        }
    }
}