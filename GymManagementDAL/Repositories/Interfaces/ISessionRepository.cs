using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {

        IEnumerable<Session> GetAllSessionWithTrainerAndCategory();

        int GetCountOfBookedSlots(int SessionId);

        Session? GetSessionWithTrainerAndCtegory(int SessionId);




    }
}
