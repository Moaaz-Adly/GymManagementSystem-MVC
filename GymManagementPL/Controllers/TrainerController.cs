using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        public IActionResult Index()
        {
            var Trainers = trainerService.GetAllTrainers();
            return View(Trainers);
        }

        public ActionResult TrainerDetails (int id)
        {
            if (id <= 0)
            {
                 TempData["ErrorMessage"] = "Id Must be greater than zero";
                return RedirectToAction(nameof(Index));
            }
            var Trainer =  trainerService.GetTrainerDetails(id);
            if (Trainer is null )
            {
                 TempData["ErrorMessage"] = "Trainer not Found";
                return RedirectToAction(nameof(Index));

            }

            return View(Trainer);
        }


        public ActionResult Create ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create (CreateTrainerViewModel CreatedTrainer)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data ans missing fields");
                return RedirectToAction(nameof (Create) , CreatedTrainer);
            }

            bool Result = trainerService.CreateTrainer(CreatedTrainer);

            if(Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Trainer can not created";

            }

            return RedirectToAction(nameof(Index));


        }


        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Must be greater than zero";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = trainerService.GetTrainerToUpdate(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not Found";
                return RedirectToAction(nameof(Index));

            }

            return View(Trainer);


        }


        [HttpPost]
        public ActionResult Edit (  int id, TrainerToUpdateViewModel UpdatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data ans missing fields");
                return RedirectToAction(nameof(Create), UpdatedTrainer);
            }

            bool Result = trainerService.UpdateTrainer(id, UpdatedTrainer);

            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Trainer can not Updated";

            }

            return RedirectToAction(nameof(Index));

        }


        public ActionResult Delete (int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Must be greater than zero";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not Found";
                return RedirectToAction(nameof(Index));

            }
            ViewBag.TrainerId = id;
            return View();

        }


        public ActionResult DeleteConfirmed ( [FromForm] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }
            bool Result = trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfuly";
            }
            else
            {
                TempData["ErrorMesage"] = "Trainer Failed To Delete";
            }
            return RedirectToAction(nameof(Index));
        }











    }
}
