namespace TeraNetSystem.Data
{
    using TeraNetSystem.Models;
    using TeraNetSystem.Data.Repositories;

    public interface ITeraNetData
    {
        IGenericRepository<ApplicationUser> Users { get; }

        IGenericRepository<Town> Towns { get; }

        IGenericRepository<Office> Offices { get; }

        IGenericRepository<News> News { get; }

        IGenericRepository<Payment> Payment { get; }

        IGenericRepository<WorkTask> Tasks { get; }

        IGenericRepository<Subscription> Subscriptions { get; }

        IGenericRepository<Request> Requests { get; }

        void SaveChanges();
    }
}
