using TeraNetSystem.Data.Repositories;
using TeraNetSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeraNetSystem.Data
{
    public class TeraNetData : ITeraNetData
    {
        private ITeraNetContext context;

        private IDictionary<Type, object> repositories;

        public TeraNetData()
            : this(new TeraNetContext())
        {
        }

        public TeraNetData(ITeraNetContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public IGenericRepository<Town> Towns
        {
            get 
            {
                return this.GetRepository<Town>();
            }
        }

        public IGenericRepository<Office> Offices
        {
            get
            {
                return this.GetRepository<Office>();
            }
        }

        public IGenericRepository<News> News
        {
            get
            {
                return this.GetRepository<News>();
            }
        }

        public IGenericRepository<Payment> Payment
        {
            get
            {
                return this.GetRepository<Payment>();
            }
        }

        public IGenericRepository<WorkTask> Tasks
        {
            get
            {
                return this.GetRepository<WorkTask>();
            }
        }

        public IGenericRepository<Abonament> Abonaments
        {
            get
            {
                return this.GetRepository<Abonament>();
            }
        }

        public IGenericRepository<Request> Requests
        {
            get
            {
                return this.GetRepository<Request>();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        private IGenericRepository<T> GetRepository<T>() where T : class
        {
            var typeOfModel = typeof(T);
            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return (IGenericRepository<T>)this.repositories[typeOfModel];
        }
    }
}
