namespace TeraNetSystem.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using TeraNetSystem.Models;

    public interface ITeraNetContext
    {
        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<Town> Towns { get; set; }

        IDbSet<Office> Offices { get; set; }

        IDbSet<News> News { get; set; }

        IDbSet<Payment> Payments { get; set; }

        IDbSet<WorkTask> Tasks { get; set; }

        IDbSet<Subscription> Subscriptions { get; set; }

        IDbSet<Request> Requests { get; set; }

        IDbSet<T> SetEntity<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
    }
}
