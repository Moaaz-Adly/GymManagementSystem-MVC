using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionServices : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {

                if (!IsCategoryValid(CreatedSession.CategoryId)) return false;
                if (!IsTrainerValid(CreatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;
                if (CreatedSession.Capasity > 25 || CreatedSession.Capasity < 0) return false;

                var SessionEntity = mapper.Map<Session>(CreatedSession);

                unitOfWork.SessionRepository.Add(SessionEntity);
                return unitOfWork.SaveChanges() > 0;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Create Session Failed : {ex}");

                return false;
            }


            
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();
            if (!Sessions.Any()) return [];

            var MappedSessions = mapper.Map<IEnumerable<Session> , IEnumerable<SessionViewModel>> (Sessions);
            foreach (var Session in MappedSessions)
                Session.AvailableSlots = Session.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id);

            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int SessionId)
        {
            var Session = unitOfWork.SessionRepository.GetSessionWithTrainerAndCtegory(SessionId);
            if (Session is null) return null;

            var MappedSession = mapper.Map<Session , SessionViewModel> (Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - unitOfWork.SessionRepository.GetCountOfBookedSlots (Session.Id);
            
            return MappedSession;
        }



        public UpdateSessionViewModel? GetSessionToUbdate(int SessionId)
        {
            var Session = unitOfWork.SessionRepository.GetById(SessionId);
            if (!IsSessionValidToUpdate(Session!)) return null;
            return mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel UpdatedSession, int SessionId)
        {
            try
            {
                var Session = unitOfWork.SessionRepository.GetById(SessionId);

                if (!IsSessionValidToUpdate(Session!)) return false;

                if (!IsTrainerValid(UpdatedSession.TrainerId)) return false;

                if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;

                mapper.Map(UpdatedSession, Session);

                Session!.UpdatedAt = DateTime.Now;

                unitOfWork.SessionRepository.Update(Session);

                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Failed {ex}");
                return false;
            }

        }


        public bool RemoveSession(int SessionId)
        {
            try
            {
                var Session = unitOfWork.SessionRepository.GetById(SessionId);
                if (IsSessionValidToRemove(Session!)) return false;
                unitOfWork.SessionRepository.Delete(Session!);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove Session Failed {ex}");
                return false;
            }
        }


        #region Helpers

        private bool IsTrainerValid (int TrainerId)
        {
            return unitOfWork.GetRepository<Trainer>().GetById (TrainerId) is not null; 
        }

        private bool IsCategoryValid (int CategoryId)
        {
            return unitOfWork.GetRepository<Category>().GetById (CategoryId) is not null; 
        }

        private bool IsDateTimeValid(DateTime StartDate , DateTime EndDate)
        {
            return StartDate < EndDate; 
        }


        private bool IsSessionValidToRemove(Session session)
        {
            if (session is null) return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            if (session.StartDate > DateTime.Now) return false;

            var ActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (ActiveBooking) return false;
            return true;
        }
        private bool IsSessionValidToUpdate(Session session)
        {
            if (session is null) return false;

            if (session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now) return false;

            var ActiveBooking = unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (ActiveBooking) return false;
            return true;
        }




        #endregion



    }
}
