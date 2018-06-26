using System.Web.Mvc;

namespace hw.easyzy.com.Areas.list
{
    public class listAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "list";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "list_default",
                "list/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "hw.easyzy.com.areas.list.Controllers" }
            );
        }
    }
}