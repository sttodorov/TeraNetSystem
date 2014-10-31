namespace TeraNetSystem.Data
{
    using System;
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using TeraNetSystem.Models;
    using TeraNetSystem.Data.Migrations;

    public class TeraNetContext : IdentityDbContext<ApplicationUser> , ITeraNetContext
    {

        public TeraNetContext()
            : base("TeraNetSystemConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TeraNetContext, Configuration>());
        }

        public IDbSet<Town> Towns { get; set; }

        public IDbSet<Office> Offices { get; set; }

        public IDbSet<News> News { get; set; }

        public IDbSet<WorkTask> Tasks { get; set; }

        public IDbSet<Payment> Payments { get; set; }


        public IDbSet<T> SetEntity<T>() where T : class
        {
            return base.Set<T>();
        }

        public static TeraNetContext Create()
        {
            return new TeraNetContext();
        }



        
    }
}
