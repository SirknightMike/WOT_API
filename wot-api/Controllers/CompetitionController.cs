using Microsoft.AspNetCore.Mvc;

namespace wot_api.Controllers
{
    public class CompetitionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
