using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DiplomServer.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<DiplomServer.Models.Problem> Problems { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.ProblemMember> ProblemMembers { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.ProjectMember> ProjectMembers { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.Salary> Salaries { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.Invite> Invites { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.oborudovanie> oborudovanies { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.ProblemChecklist> ProblemChecklists { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.ProblemComment> ProblemComments { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.TasksFile> TasksFiles { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.ProjectFile> ProjectFiles { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.UserInfo> UserInfoes { get; set; }

        public System.Data.Entity.DbSet<DiplomServer.Models.ProjectInfo> ProjectInfoes { get; set; }
    }
}