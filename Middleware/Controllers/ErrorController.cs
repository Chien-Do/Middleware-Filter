using Microsoft.AspNetCore.Mvc;

namespace Middleware.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            return Problem();
        }
    }
}
