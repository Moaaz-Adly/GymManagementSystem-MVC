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

        public MemberService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public bool CreateMember(CreateMemberViewModel CreatedMember)
        {
            try
            {
               if (EmailExist(CreatedMember.Email) || PhoneExist(CreatedMember.Phone)) return false;
                var Member = new Member()
                {
                    Name = CreatedMember.Name,
                    Email = CreatedMember.Email,
                    Phone = CreatedMember.Phone,
                    Gender = CreatedMember.Gender,
                    DateOfBirth = CreatedMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNo = CreatedMember.BuildingNumber,
                        City = CreatedMember.City,
                        Street = CreatedMember.Street,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = CreatedMember.HealthRecordViewModel.Height,
                        Weight = CreatedMember.HealthRecordViewModel.Weight,
                        BloodType = CreatedMember.HealthRecordViewModel.BloodType,
                        Note = CreatedMember.HealthRecordViewModel.Note
                    }

                };
                 unitOfWork.GetRepository<Member>().Add(Member);
                return unitOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {
                return false;
            }
            


           
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];
            var MemberViewModels = new List<MemberViewModel>();
            foreach (var member in members)
            {
                var memberviewmodel = new MemberViewModel()
                {
                    Id = member.Id,
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Gender = member.Gender.ToString(),
                    Photo = member.Photo
                };
                MemberViewModels.Add(memberviewmodel);
            }
            return MemberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            var memberviewmodel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                Photo = Member.Photo,
                DateofBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BuildingNo} - {Member.Address.Street} - {Member.Address.City} ",

                
            };
            var ActiveMemberhip = unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.MemberId == MemberId)
                .FirstOrDefault();
            if(ActiveMemberhip is not null)
            {
                memberviewmodel.MemberShipStartDate = ActiveMemberhip.CreatedAt.ToShortDateString();
                memberviewmodel.MemberShipEndDate = ActiveMemberhip.EndDate.ToShortDateString();
                var Plan = unitOfWork.GetRepository<Plan>().GetById(ActiveMemberhip.PlanId);
                memberviewmodel.PlanName = Plan?.Name;

            }
            return memberviewmodel;

        }

        public HealthRecodViewModel? GetMemberHealthRecod(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

            return new HealthRecodViewModel()
            {
                BloodType = Member.HealthRecord.BloodType,
                Weight = Member.HealthRecord.Weight,
                Height = Member.HealthRecord.Height,
                Note = Member.HealthRecord.Note,
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = Member.Name,
                Phone = Member.Phone,
                Photo = Member.Photo,
                BuildingNumber = Member.Address.BuildingNo,
                City = Member.Address.City,
                Street = Member.Address.Street,
                Email = Member.Email
            };
        }

        public bool RemoveMember(int MemberId)
        {
            var Member = unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return false;
            var HasActiveSession = unitOfWork.GetRepository<MemberSession>().GetAll(x=>x.MemberId == MemberId && x.Session.StartDate > DateTime.Now ).Any();
            if (HasActiveSession) return false;

            var MemberShips = unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId);
            try
            {
                if (MemberShips.Any())
                {
                    foreach( var membership in MemberShips)
                    {
                        unitOfWork.GetRepository<MemberShip>().Delete(membership);
                    }

                }
                 unitOfWork.GetRepository<Member>().Delete(Member) ;
                return unitOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                if (EmailExist(UpdatedMember.Email) || PhoneExist(UpdatedMember.Phone)) return false;
                var Member = unitOfWork.GetRepository<Member>().GetById(Id);
                if (Member is null) return false;
                Member.Email = UpdatedMember.Email;
                Member.Phone = UpdatedMember.Phone;
                Member.Address.BuildingNo = UpdatedMember.BuildingNumber;
                Member.Address.City = UpdatedMember.City;
                Member.Address.Street = UpdatedMember.Street;
                Member.UpdatedAt = DateTime.Now;
                 unitOfWork.GetRepository<Member>().Update(Member) ;
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }





        }








        #region Helper

        private bool EmailExist (string Email)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.Email == Email).Any();
        }
        private bool PhoneExist (string Phone)
        {
            return unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == Phone).Any();
        }


        #endregion





    }
}
