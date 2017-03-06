using Microsoft.AspNetCore.Mvc;

namespace BowlingGame.Controllers.Web
{
    public class GameController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}