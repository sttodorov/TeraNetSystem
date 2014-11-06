namespace TeraNetSystem.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using TeraNetSystem.Models;

    public class GenericRepository<T> : IGenericRepository<T>
        where T : class, IEntityProtectedDelete
    {
        private ITeraNetContext context;

        public GenericRepository(ITeraNetContext context)
        {
            this.context = context;
        }

        public IQueryable<T> All()
        {
            return this.context.SetEntity<T>().AsParallel().Where(c => c.IsDeleted == false).AsQueryable();
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions)
        {
            return this.All().Where(conditions);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public T Delete(T entity)
        {
            if(entity is IEntityProtectedDelete)
            {
                (entity as IEntityProtectedDelete).IsDeleted = true;
                (entity as IEntityProtectedDelete).DateDeleted = DateTime.Now;
                this.ChangeState(entity, EntityState.Modified);
            }
            else
            {
                this.ChangeState(entity, EntityState.Deleted);
            }
            return entity;
        }

        public void Detach(T entity)
        {
            this.ChangeState(entity, EntityState.Detached);
        }

        private void ChangeState(T entity, EntityState state)
        {
            this.context.SetEntity<T>().Attach(entity);
            this.context.Entry(entity).State = state;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
