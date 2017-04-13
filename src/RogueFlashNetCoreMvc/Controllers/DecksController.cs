using Microsoft.AspNetCore.Mvc;
using RogueFlashNetCoreMvc.Model;

namespace RogueFlashNetCoreMvc.Controllers
{
    public class DecksController : AbstractController
    {
        public DecksController(AppDbContext dbContext)
            : base(dbContext)
        {
            //
        }


        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Title"] = "Decks";

            return View();
        }
    }
}
