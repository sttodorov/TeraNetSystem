using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Collections.Concurrent;

using CaptchaMvc.Infrastructure;
using CaptchaMvc.Interface;

namespace TeraNetSystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string MultipleParameterKey = "_multiple_";

        private static readonly ConcurrentDictionary<int, ICaptchaManager> CaptchaManagers =
            new ConcurrentDictionary<int, ICaptchaManager>();

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CaptchaUtils.CaptchaManager.InputElementName = "RequstCaptcha";
        }

        private static ICaptchaManager GetCaptchaManager(IParameterContainer parameterContainer)
        {
            int numberOfCaptcha;
            if (parameterContainer.TryGet(MultipleParameterKey, out numberOfCaptcha))
                return CaptchaManagers.GetOrAdd(numberOfCaptcha, CreateCaptchaManagerByNumber);

            //If not found parameter return default manager.
            return CaptchaUtils.CaptchaManager;
        }

        private static ICaptchaManager CreateCaptchaManagerByNumber(int i)
        {
            var captchaManager = new DefaultCaptchaManager(new SessionStorageProvider());
            captchaManager.ImageElementName += i;
            captchaManager.InputElementName += i;
            captchaManager.TokenElementName += i;
            captchaManager.ImageUrlFactory = (helper, pair) =>
            {
                var dictionary = new RouteValueDictionary();
                dictionary.Add(captchaManager.TokenParameterName, pair.Key);
                dictionary.Add(MultipleParameterKey, i);
                return helper.Action("Generate", "DefaultCaptcha", dictionary);
            };
            captchaManager.RefreshUrlFactory = (helper, pair) =>
            {
                var dictionary = new RouteValueDictionary();
                dictionary.Add(MultipleParameterKey, i);
                return helper.Action("Refresh", "DefaultCaptcha", dictionary);
            };
            return captchaManager;
        }

    }
}
