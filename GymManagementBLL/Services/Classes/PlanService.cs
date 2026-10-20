using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classses
{
    public class PlanService : IPlanService

    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork _unitOfWork , IMapper mapper) 
        {
            unitOfWork = _unitOfWork;
            this.mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];

            //return Plans.Select(x => new PlanViewModel()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Description = x.Description,
            //    Price = x.Price,
            //    DurationDays = x.DurationDays,
            //    IsActive = x.IsActive
            //});

            var MappesPlans = mapper.Map<IEnumerable<PlanViewModel>>(Plans);
            return MappesPlans;

        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null) return null;
            //return new PlanViewModel()
            //{
            //    Id = PlanId,
            //    Name = Plan.Name,
            //    Description = Plan.Description,
            //    Price = Plan.Price,
            //    DurationDays = Plan.DurationDays,
            //    IsActive = Plan.IsActive
            //};
            var PlanViewModel = mapper.Map<PlanViewModel>(Plan);
            return PlanViewModel;

        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null|| Plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;

            //return new UpdatePlanViewModel()
            //{
            //    Description = Plan.Description,
            //    DurationDays= Plan.DurationDays,
            //    Price = Plan.Price,
            //    PlanName = Plan.Name
            //};

            return mapper.Map<UpdatePlanViewModel>(Plan);

        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatedPlan)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMemberShip(PlanId)) return false;
            try
            {
                mapper.Map(UpdatedPlan , Plan);

                unitOfWork.GetRepository<Plan>().Update(Plan);
                return unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }
           
        }

        public bool ToggleStatus(int PlanId)
        {
            var Plan = unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMemberShip(PlanId)) return false ;
            Plan.IsActive = Plan.IsActive == true ? false : true ;
            Plan.UpdatedAt = DateTime.Now;
            try
            {
                unitOfWork.GetRepository<Plan>().Update(Plan);
                return unitOfWork.SaveChanges()>0;
            }
            catch
            {
                return false;
            }
        }

      


        private bool HasActiveMemberShip(int PlanId)
        {
            var ActiveMemberShip = unitOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active");
            return ActiveMemberShip.Any();
            
        }
    }
}
