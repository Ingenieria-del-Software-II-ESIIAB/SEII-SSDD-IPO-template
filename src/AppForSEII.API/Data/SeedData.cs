namespace AppForSEII.API.Data {
    public class SeedData {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger) {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex) {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

 

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles) {

            foreach (string roleName in roles) {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result) {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {

            List<ApplicationUser> users = new List<ApplicationUser> { 
                new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es"), 
                new ApplicationUser("2", "Aurora", "Macias", "aurora@uclm.es"),
                new ApplicationUser("3", "Clara", "Ayora", "claran@uclm.es")                
            
            };

            List<string> rolesNames = new List<string> { "Password1234$", "OtherPass12$", "OtherPass12$" };
            
            for (int i = 0; i < users.Count; i++)
            {
                SeedUser(userManager, roles, users[i], roles[i], "OtherPass12$");
            }


        }

        private static void SeedUser(UserManager<ApplicationUser> userManager, List<string> roles, ApplicationUser user, string role, string password)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync(user.UserName).Result == null)
            {                
                user.EmailConfirmed = true;
                var result = userManager.CreateAsync(user, password);
                result.Wait();
                if (result.IsCompletedSuccessfully)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user, role).Wait();
                }
            }
        }
    }
}