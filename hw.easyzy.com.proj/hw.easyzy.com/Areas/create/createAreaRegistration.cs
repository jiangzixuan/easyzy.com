using System.Web.Mvc;

namespace hw.easyzy.com.Areas.create
{
    public class createAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "create";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "create_default",
                "create/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "hw.easyzy.com.areas.create.Controllers" }
            );
        }
    }
}