using GymManagementBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        #region Index
        public ActionResult Index()
        {
            var Session = sessionService.GetAllSessions();

            return View(Session);
        } 
        #endregion

        #region Details

        public ActionResult Details(int id)
        {

            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Must Be Greater Than Zero";
                return RedirectToAction(nameof(Index));
            }

            var Session = sessionService.GetSessionById(id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Invalid Session";
                return RedirectToAction(nameof(Index));

            }

            return View(Session);
        }
        #endregion

        #region Create

        public ActionResult Create()
        {
            LoadDropDownForCategorys();
            LoadDropDownForTrainers();
            return View();

        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel CreatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownForCategorys();
                LoadDropDownForTrainers();
                return View(CreatedSession);
            }

            var Result = sessionService.CreateSession(CreatedSession);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To Create ";
                LoadDropDownForCategorys();
                LoadDropDownForTrainers();
                return View(CreatedSession);
            }
        } 
        #endregion

        #region Edit 
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = sessionService.GetSessionToUbdate(id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownForTrainers();
            return View(Session);

        }

        [HttpPost]
        public ActionResult Edit(int id, UpdateSessionViewModel updatedSession)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Data Not Valid";
                return RedirectToAction(nameof(Index));
            }
            var Result = sessionService.UpdateSession(updatedSession, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session can not Updated";
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Delete 

        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Data Not Valid";
                return RedirectToAction(nameof(Index));
            }
            var Session = sessionService.GetSessionById(id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();

        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = sessionService.RemoveSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Removed Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session can not Removed";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion




        #region Helper Methods 
        private void LoadDropDownForTrainers()
        {
            var Trainers = sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        private void LoadDropDownForCategorys()
        {
            var Categorys = sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categorys, "Id", "Name");
        }
        #endregion
    }
}
