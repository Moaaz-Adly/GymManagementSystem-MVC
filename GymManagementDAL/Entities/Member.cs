using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        #region props
        //joinDate => CreatedAt in BaseEntity

        public string Photo { get; set; } = null!;
        #endregion

        #region Relation Ships

        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<MemberShip> MemberShips { get; set; } = null!;

        public ICollection<MemberSession> MemberSessions { get; set; } = null!;


        #endregion












    }
}
