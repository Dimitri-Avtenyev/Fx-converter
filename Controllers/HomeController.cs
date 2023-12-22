using Microsoft.AspNetCore.Mvc;

namespace Fx_converter.Controllers
{
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        [HttpGet] 
        public IActionResult Index() {

            var response = new { message = "Welcome to GrantThornton excel processing of foreign exchange values!"};

            return Ok(response);
        }
   
    }
}
