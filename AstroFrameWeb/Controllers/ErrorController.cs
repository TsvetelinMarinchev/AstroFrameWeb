using Microsoft.AspNetCore.Mvc;

namespace AstroFrameWeb.Controllers
{
    public class ErrorController : Controller
    {

        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("Error/500")]
        public IActionResult Error500()
        {
            return View();
        }
    }
}
