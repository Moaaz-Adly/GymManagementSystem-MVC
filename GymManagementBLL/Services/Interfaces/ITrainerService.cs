using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        TrainerViewModel? GetTrainerDetails(int TrainerId);

        bool CreateTrainer(CreateTrainerViewModel CreatedTrainer);

        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId);

        bool UpdateTrainer(int TrainerId , TrainerToUpdateViewModel UpdatedTrainer );

        bool RemoveTrainer(int TrainerId);

        
















    }
}
