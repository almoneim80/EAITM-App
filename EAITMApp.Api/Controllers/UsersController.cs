using Microsoft.AspNetCore.Mvc;

namespace EAITMApp.Api.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
