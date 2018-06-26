using System.Web.Mvc;

namespace hw.easyzy.com.Areas.submit
{
    public class submitAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "submit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "submit_default",
                "submit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "hw.easyzy.com.areas.submit.Controllers" }
            );
        }
    }
}