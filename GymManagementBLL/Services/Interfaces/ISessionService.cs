using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int SessionId);

        bool CreateSession(CreateSessionViewModel CreatedSession);

        UpdateSessionViewModel? GetSessionToUbdate(int SessionId);

        bool UpdateSession(UpdateSessionViewModel UpdatedSession, int SessionId);

        bool RemoveSession(int SessionId);


    }
}
