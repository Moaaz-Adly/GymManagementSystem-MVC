using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers =  unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];
            
            var TrainersViewModels = new List<TrainerViewModel>();
            foreach (var Trainer in Trainers)
            {
                var TrainerViewModel = new TrainerViewModel()
                {
                    Id = Trainer.Id,
                    Name = Trainer.Name,
                    Email = Trainer.Email,
                    Phone = Trainer.Phone,
                    Specialties = Trainer.Specialties.ToString(),
                };
                TrainersViewModels.Add(TrainerViewModel);
            }
            return TrainersViewModels;

            
        }
        public bool CreateTrainer(CreateTrainerViewModel CreatedTrainer)
        {
            if (EmailExist(CreatedTrainer.Email) || PhoneExist(CreatedTrainer.Phone)) return false;

            try
            {
                var Trainer = new Trainer()
                {
                    Name = CreatedTrainer.Name,
                    Email = CreatedTrainer.Email,
                    Phone = CreatedTrainer.Phone,
                    DateOfBirth = CreatedTrainer.DateOfBirth,
                    Specialties = CreatedTrainer.Specialties,
                    Gender = CreatedTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNo = CreatedTrainer.BuildingNumber,
                        City = CreatedTrainer.City,
                        Street = CreatedTrainer.Street,
                    }

                };
                unitOfWork.GetRepository<Trainer>().Add(Trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false ;
            }





        }
        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null  ) return null;
            return new TrainerViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Specialties = Trainer.Specialties.ToString(),
                DateOfBirth = Trainer.DateOfBirth.ToShortDateString(),
                Address = $"{Trainer.Address.BuildingNo} - {Trainer.Address.Street} - {Trainer.Address.City}"
            };




        }
        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null ) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Street = Trainer.Address.Street,
                City = Trainer.Address.City,
                BuildingNumber = Trainer.Address.BuildingNo,
                Specialties = Trainer.Specialties
            };







        }
        public bool RemoveTrainer(int TrainerId)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null || HasActiveSessions(TrainerId)) return false;

            unitOfWork.GetRepository<Trainer>().Delete(Trainer);
            return unitOfWork.SaveChanges() > 0;



        }
        public bool UpdateTrainer(int TrainerId, TrainerToUpdateViewModel UpdatedTrainer)
        {
            var Trainer = unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null ) return false;

            if (EmailExist(UpdatedTrainer.Email) || PhoneExist(UpdatedTrainer.Phone) ) return false;

            try
            {
                Trainer.Email = UpdatedTrainer.Email;
                Trainer.Phone = UpdatedTrainer.Phone;
                Trainer.Address.BuildingNo = UpdatedTrainer.BuildingNumber;
                Trainer.Address.Street = UpdatedTrainer.Street;
                Trainer.Address.City = UpdatedTrainer.City;
                Trainer.Specialties = UpdatedTrainer.Specialties;
                Trainer.UpdatedAt = DateTime.Now;

                unitOfWork.GetRepository<Trainer>().Update(Trainer);
                return unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }




        }






        #region Helper

        private bool EmailExist(string Email)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(x => x.Email == Email).Any();
        }
        private bool PhoneExist(string Phone)
        {
            return unitOfWork.GetRepository<Trainer>().GetAll(x => x.Phone == Phone).Any();
        }
        private bool HasActiveSessions(int Id)
        {
            var activeSessions = unitOfWork.GetRepository<Session>().GetAll(
               s => s.TrainerId == Id && s.StartDate > DateTime.Now).Any();
            return activeSessions;
        }


        #endregion
    }
}
