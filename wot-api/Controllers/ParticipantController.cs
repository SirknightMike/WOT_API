using Microsoft.AspNetCore.Mvc;

namespace wot_api.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
