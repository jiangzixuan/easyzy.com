using System.Web.Mvc;

namespace hw.easyzy.com.Areas.analyze
{
    public class analyzeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "analyze";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "analyze_default",
                "analyze/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "hw.easyzy.com.areas.analyze.Controllers" }
            );
        }
    }
}