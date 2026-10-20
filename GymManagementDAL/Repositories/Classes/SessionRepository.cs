using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenaricRepository<Session>, ISessionRepository
    {
        private readonly GYMDbContext dbContext;

        public SessionRepository(GYMDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionWithTrainerAndCategory()
        {
            return dbContext.Sessions.Include(x => x.SessionTrainer)
                                     .Include(x => x.Category)
                                     .ToList();


        }

        public int GetCountOfBookedSlots(int SessionId)
        {
            return dbContext.MemberSessions.Count(x => x.SessionId == SessionId);
        }

        public Session? GetSessionWithTrainerAndCtegory(int SessionId)
        {
            return dbContext.Sessions.Include(x=>x.SessionTrainer)
                .Include(x=>x.Category)
                .FirstOrDefault(x=>x.Id == SessionId );

        }
    }
}
