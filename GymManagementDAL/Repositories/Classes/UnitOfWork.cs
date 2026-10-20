using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type , object> Repositories = new();
        private readonly GYMDbContext dbContext;

        public UnitOfWork(GYMDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);


            if (Repositories.TryGetValue(EntityType, out var Repo))
                return (IGenaricRepository<TEntity>)Repo;


            var NewRepo = new GenaricRepository<TEntity>(dbContext);
            Repositories[EntityType] = NewRepo;
            return NewRepo;

        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }
    }
}
