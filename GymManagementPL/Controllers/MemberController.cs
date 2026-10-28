using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;

        public MemberController(IMemberService memberService)
        {
            this.memberService = memberService;
        }
        #region Get All

        public ActionResult Index()
        {
            var Members = memberService.GetAllMembers();

            return View(Members);
        }

        #endregion


        #region Deatils

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }


            var Member = memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));

            var HealthRecord = memberService.GetMemberHealthRecord(id);
            if (HealthRecord is null)
                return RedirectToAction(nameof(Index));
            return View(HealthRecord);
        } 
        #endregion

        #region Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Ckeck Data And Missing Fields");
                return RedirectToAction(nameof(Create), CreatedMember);

            }


            bool Result = memberService.CreateMember(CreatedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfuly";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create";
            }

            return RedirectToAction(nameof(Index));



        }


        #endregion

        #region Edit


        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }


            var Member = memberService.GetMemberToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Member);
        }

        [HttpPost]
        public ActionResult Edit (int id , MemberToUpdateViewModel EditedMember)
        {
            if(!ModelState.IsValid)
            {
                return View(EditedMember);
            }
            bool Result = memberService.UpdateMemberDetails(id, EditedMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfuly";
            }
            else
            {
                TempData["ErrorMesage"] = "Member Failed To Update";
            }
            return RedirectToAction(nameof(Index));


        }







        #endregion




        #region Delete

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }


            var Member = memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberId = id;

            return View();


        }


        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }
            bool Result = memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfuly";
            }
            else
            {
                TempData["ErrorMesage"] = "Member Failed To Delete";
            }
            return RedirectToAction(nameof(Index));



        }




        #endregion




    }
}
