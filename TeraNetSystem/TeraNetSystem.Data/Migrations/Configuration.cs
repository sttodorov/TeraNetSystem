namespace TeraNetSystem.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using TeraNetSystem.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TeraNetContext>
    {
        private const string ADMIN_ROLE = "Admin";
        private const string OFFICE_ROLE = "OfficeMan";
        private const string NETWORK_ROLE = "NetworkMan";

        private const string DEFAULT_SEED_PASSWORD = "123456";
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TeraNetSystem.Data.TeraNetContext context)
        {
            if (!context.Towns.Any())
            {
                context.Towns.Add(new Town() { TownName = "Sofia" });
                context.Towns.Add(new Town() { TownName = "Plovdiv" });
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roleManager.Create(new IdentityRole() { Name = ADMIN_ROLE });
                roleManager.Create(new IdentityRole() { Name = OFFICE_ROLE });
                roleManager.Create(new IdentityRole() { Name = NETWORK_ROLE });
            }

            if (!context.Users.Any())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var sofiaTownFromDb = context.Towns.FirstOrDefault(t => t.TownName == "Sofia");

                var administartor = new ApplicationUser() 
                { 
                    UserName = "admin@teranet.com", 
                    Email = "admin@teranet.com",  
                    FirstName = ADMIN_ROLE,
                    LastName = ADMIN_ROLE,
                    Town = sofiaTownFromDb,
                    Address = "Vitosha str. 18" 
                };
                userManager.Create(administartor, DEFAULT_SEED_PASSWORD);
                userManager.AddToRole(administartor.Id, ADMIN_ROLE);

                var officeMan = new ApplicationUser()
                {
                    UserName = "office@teranet.com",
                    Email = "office@teranet.com",
                    FirstName = OFFICE_ROLE,
                    LastName = OFFICE_ROLE,
                    Town = sofiaTownFromDb,
                    Address = "Vitosha str. 18"
                };
                userManager.Create(officeMan, DEFAULT_SEED_PASSWORD);
                userManager.AddToRole(officeMan.Id, OFFICE_ROLE);

                var networkMan = new ApplicationUser()
                {
                    UserName = "network@teranet.com",
                    Email = "network@teranet.com",
                    FirstName = NETWORK_ROLE,
                    LastName = NETWORK_ROLE,
                    Town = sofiaTownFromDb,
                    Address = "Vitosha str. 18"
                };
                userManager.Create(networkMan, DEFAULT_SEED_PASSWORD);
                userManager.AddToRole(networkMan.Id, NETWORK_ROLE);
            }
        }
    }
}
