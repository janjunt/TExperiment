using System.Web.Mvc;

namespace TExperiment.Web.Areas.EsModule
{
    public class EsModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EsModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EsModule_default",
                "EsModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}