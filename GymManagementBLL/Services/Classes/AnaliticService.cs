using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classses
{
    public class AnaliticService : IAnaliticService
    {
        private readonly IUnitOfWork unitOfWork;

        public AnaliticService(IUnitOfWork unitOfWork )
        {
            this.unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnaliticDate()
        {
            var Sessions = unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel()
            {
                 ActiveMembers = unitOfWork.GetRepository<MemberShip>().GetAll(x=>x.Status =="Active").Count(),
                 TotalMembers = unitOfWork.GetRepository<Member>().GetAll().Count(),
                 TotalTrainers = unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                 UpcomingSessions = Sessions.Count(x=>x.StartDate > DateTime.Now),
                 OngoingSessions = Sessions.Count(x=>x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now),
                 CompletedSessions = Sessions.Count(x=>x.EndDate < DateTime.Now),

            };
        }
    }
}
