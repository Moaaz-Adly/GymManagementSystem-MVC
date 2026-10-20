using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Session : BaseEntity
    {
        #region props

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #endregion

        #region Relation Ships


        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;



        public int TrainerId { get; set; }
        public Trainer SessionTrainer { get; set; } = null!;


        public ICollection<MemberSession> MemberSessions { get; set; } = null!;



        #endregion









    }
}
