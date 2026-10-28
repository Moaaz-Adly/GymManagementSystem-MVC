using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService planService;

        public PlanController(IPlanService planService)
        {
            this.planService = planService;
        }
        public IActionResult Index()
        {
            var Plans = planService.GetAllPlans();
            return View(Plans);
        }


        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id Must Be Greater Than Zero";
                return RedirectToAction(nameof(Index));
            }
            var Plan = planService.GetPlanById(id);
            if (Plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));

            }

            return View(Plan);
        }

        public ActionResult Edit (int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Must Be Greater Than Zero";
                return RedirectToAction(nameof(Index));
            }
            var Plan = planService.GetPlanToUpdate(id);
            if (Plan is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Updated";
                return RedirectToAction(nameof(Index));

            }

            return View(Plan);

        }

        [HttpPost]
        public ActionResult Edit(int id ,UpdatePlanViewModel UpdatedPlan  )
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Plan Can Not Updated");
                return RedirectToAction(nameof(Index));
            }
            bool Result = planService.UpdatePlan(id, UpdatedPlan);
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Succesfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Can Not Updated";
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public ActionResult Activate(int id)
        {
            var Result = planService.ToggleStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Changed Plan Status";
            }
            return RedirectToAction(nameof(Index));



        }











    }
}
