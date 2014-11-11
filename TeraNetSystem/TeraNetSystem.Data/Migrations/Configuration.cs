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
                context.Towns.Add(new Town() { TownName = "Sofia" });
                context.Towns.Add(new Town() { TownName = "Plovdiv" });
                context.SaveChanges();
            }
            if (!context.Subscriptions.Any())
            {
                context.Subscriptions.Add(new Subscription() 
                {
                    SubscriptionName = "Default",
                    DownloadSpeed = 50,
                    UploadSpeed = 35,
                    Price = 20.00m,
                    Description = "Default Subscription. With 50Mb download speed and #0Mb upload speed! Unlimeted usage!" 
                });
                context.Subscriptions.Add(new Subscription()
                {
                    SubscriptionName = "Business",
                    DownloadSpeed = 100,
                    UploadSpeed = 75,
                    Price = 50.00m,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                });
            }
            if (!context.Offices.Any())
            {
                var sofiaTownFromDb = context.Towns.FirstOrDefault(t => t.TownName == "Sofia");

                var newOffice = new Office()
                {
                    Name= "Mladost 1",
                    Town = sofiaTownFromDb,
                    Address = "Maldost, Atanas Manchev 122",
                    Phone = "0888555444",
                    ImagePath = "~/Content/images/office.png"
                };
                context.Offices.Add(newOffice);

                var anotherOffice = new Office()
                {
                    Name = "Student town",
                    Town = sofiaTownFromDb,
                    Address = "Student town, 8th December 63",
                    Phone = "0877111222",
                    ImagePath = "~/Content/images/office.png"
                };

                context.Offices.Add(anotherOffice);
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
                var defaultSubscription = context.Subscriptions.FirstOrDefault(a => a.SubscriptionName == "Default");
                var sofiaOffice = context.Offices.FirstOrDefault(a => a.Name == "Mladost 1");

                var administartor = new ApplicationUser()
                {
                    UserName = "admin@teranet.com",
                    Email = "admin@teranet.com",
                    FirstName = ADMIN_ROLE,
                    LastName = ADMIN_ROLE,
                    Town = sofiaTownFromDb,
                    Address = "Vitosha str. 18",
                    Subscription = defaultSubscription,
                    Office = sofiaOffice
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
                    Subscription = defaultSubscription,
                    Office = sofiaOffice
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
