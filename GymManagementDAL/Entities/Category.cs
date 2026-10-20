using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;


        #region Relation Ships

        public ICollection<Session> Sessions { get; set; } = null!;




        #endregion
    }
}
