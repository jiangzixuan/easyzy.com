using System.Web.Mvc;

namespace hw.easyzy.com.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        
    }
}