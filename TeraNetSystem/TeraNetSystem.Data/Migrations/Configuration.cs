namespace TeraNetSystem.Data.Migrations
{

    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using TeraNetSystem.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TeraNetSystem.Data.TeraNetContext>
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
                context.Towns.Add(new Town() { TownName = "Sofia"});
                context.Towns.Add(new Town() { TownName = "Plovdiv" });
                context.SaveChanges();
            }
            if (!context.Subscriptions.Any())
            {
                context.Subscriptions.Add(new Subscription() { SubscriptionName = "Default", MB = 50, Price = 20.00m, Description = "Default Subscription. With 50Mb download speed and #0Mb upload speed! Unlimeted usage!" });
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
                var defaultSubscription = context.Subscriptions.FirstOrDefault(a => a.SubscriptionName == "Default");

                var administartor = new ApplicationUser()
                {
                    UserName = "admin@teranet.com",
                    Email = "admin@teranet.com",
                    FirstName = ADMIN_ROLE,
                    LastName = ADMIN_ROLE,
                    Town = sofiaTownFromDb,
                    Address = "Vitosha str. 18",
                    Subscription = defaultSubscription
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
                    Address = "Vitosha str. 18",
                    Subscription = defaultSubscription
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
                    Address = "Vitosha str. 18",
                    Subscription = defaultSubscription
                };
                userManager.Create(networkMan, DEFAULT_SEED_PASSWORD);
                userManager.AddToRole(networkMan.Id, NETWORK_ROLE);
            }
            if (!context.Offices.Any())
            {
                var sofiaTownFromDb = context.Towns.FirstOrDefault(t => t.TownName == "Sofia");

                var newOffice = new Office()
                {
                    Town = sofiaTownFromDb,
                    Address = "Maldost, Atanas Manchev 122",
                    Phone = "0888555444"
                };
                context.Offices.Add(newOffice);

                var anotherOffice = new Office()
                {
                    Town = sofiaTownFromDb,
                    Address = "Studentski grad, 8mi Dekemvri",
                    Phone="0877111222"
                };

                context.Offices.Add(anotherOffice);

                context.SaveChanges();
            }
            if (!context.Payments.Any())
            {
                var officeFromDb = context.Offices.FirstOrDefault(o => o.Address == "Maldost, Atanas Manchev 122");
                var admin = context.Users.FirstOrDefault(u => u.UserName == "admin@teranet.com");

                context.Payments.Add(new Payment()
                {
                        Client = admin,
                        Office = officeFromDb,
                        PerMonth = Month.April,
                });

                context.Payments.Add(new Payment()
                {
                    Client = admin,
                    Office = officeFromDb,
                    PerMonth = Month.March,
                });

                context.Payments.Add(new Payment()
                {
                    Client = admin,
                    Office = officeFromDb,
                    PerMonth = Month.July,
                });

                context.SaveChanges();
            }
        }

    }
}
