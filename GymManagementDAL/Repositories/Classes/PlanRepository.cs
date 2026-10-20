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
    public class PlanRepository : IPlanRepository
    {
        private readonly GYMDbContext dbContext;

        public PlanRepository(GYMDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Plan> GetAll() => dbContext.Plans.ToList();

        public Plan? GetById(int id) => dbContext.Plans.Find(keyValues: id);

        public int Update(Plan plan)
        {
            dbContext.Plans.Add(entity: plan);
            return dbContext.SaveChanges();
        }
    }
}
