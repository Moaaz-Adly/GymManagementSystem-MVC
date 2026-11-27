using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnaliticService analiticService;

        public HomeController(IAnaliticService analiticService)
        {
            this.analiticService = analiticService;
        }
        public IActionResult Index()
        {
            var Data = analiticService.GetAnaliticDate();
            return View(Data);
        }
    }
}
