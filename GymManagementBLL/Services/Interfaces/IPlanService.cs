using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        public IEnumerable<PlanViewModel> GetAllPlans();

        public PlanViewModel? GetPlanById(int PlanId);

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId);
        public bool UpdatePlan(int  PlanId, UpdatePlanViewModel UpdatedPlan );

        public bool ToggleStatus (int PlanId);

    }
}
