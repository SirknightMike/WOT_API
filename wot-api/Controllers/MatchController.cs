using Microsoft.AspNetCore.Mvc;

namespace wot_api.Controllers
{
    public class MatchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
