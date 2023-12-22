using Microsoft.AspNetCore.Mvc;

namespace Fx_converter.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet] 
        public IActionResult Index() {

            var response = new { message = "Welcome to GrantThornton excel processing of foreign exchange values!"};
            return Json(response);
        }
    }
}
