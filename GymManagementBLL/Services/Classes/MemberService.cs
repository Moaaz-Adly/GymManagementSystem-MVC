using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool CreateMember(CreateMemberViewModel CreatedMember)
        {
            try
            {

                if (IsEmailExist(CreatedMember.Email) || IsPhoneExist(CreatedMember.Phone)) return false;

                //var PhotoName = attachmentService.Upload("members", CreatedMember.PhotoFile);
                //if (string.IsNullOrEmpty(PhotoName)) return false;

                var member = mapper.Map<Member>(CreatedMember);
                //member.Photo = PhotoName;
                unitOfWork.GetRepository<Member>().Add(member);

                var IsCreated = unitOfWork.SaveChanges() > 0;
                if (!IsCreated)
                {
                    //attachmentService.Delete(PhotoName, "members");
                    return false;
                }
                else
                {
                    return IsCreated;
                }



            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any()) return [];

            var MemberViewModels = mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;

        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

            var ViewModel = mapper.Map<MemberViewModel>(Member);
            var ActiveMemberShip = unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId && x.Status == "Active")
                                                        .FirstOrDefault();
            if (ActiveMemberShip is not null)
            {

                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                var Plan = unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;
            }
            return ViewModel;

        }

        public HealthRecordViewModel? GetMemberHealthRecord(int MemberId)
        {
            var MemberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;
            return mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            return mapper.Map<MemberToUpdateViewModel>(Member);
        }

        public bool RemoveMember(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return false;




            var SessionIds = unitOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId).Select(x => x.SessionId);

            var HasFutureSession = unitOfWork.GetRepository<Session>()
                .GetAll(x => SessionIds.Contains(x.Id) && x.StartDate > DateTime.Now).Any();

            if (HasFutureSession) return false;



            var MemberShips = unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId);
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)
                    {
                        unitOfWork.GetRepository<MemberShip>().Delete(memberShip);
                    }
                }

                unitOfWork.GetRepository<Member>().Delete(Member);
                var IsDeleted = unitOfWork.SaveChanges() > 0;

                //if (IsDeleted)
                //    attachmentService.Delete(Member.Photo, "members");

                return IsDeleted;


            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel updatedMember)
        {
            var EmailExist = unitOfWork.GetRepository<Member>()
                .GetAll(x => x.Email == updatedMember.Email && x.Id != MemberId);

            var PhoneExist = unitOfWork.GetRepository<Member>()
                .GetAll(x => x.Phone == updatedMember.Phone && x.Id != MemberId);
            if (EmailExist.Any() || PhoneExist.Any()) return false;

            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return false;
            mapper.Map(updatedMember, Member);
            unitOfWork.GetRepository<Member>().Update(Member);
            return unitOfWork.SaveChanges() > 0;
        }





        #region Helper

        private bool IsEmailExist (string Email)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.Email == Email).Any();
        }
        private bool IsPhoneExist (string Phone)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == Phone).Any();
        }


        #endregion





    }
}
