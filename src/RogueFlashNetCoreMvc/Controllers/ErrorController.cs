using Microsoft.AspNetCore.Mvc;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
