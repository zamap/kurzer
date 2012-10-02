using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using KuerzerModels;

namespace KuerzerRepositories
{
    public class KuerzerDbContext : DbContext 
    {

		public KuerzerDbContext(): base(nameOrConnectionString: "DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Use singular table names
          //  modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

           // modelBuilder.Configurations.Add(new SessionConfiguration());
          //  modelBuilder.Configurations.Add(new AttendanceConfiguration());
		
			modelBuilder.Entity<UserApplication>()
				.HasRequired(s=>s.UserProfile).WithMany(s=>s.UserApplications).WillCascadeOnDelete(true);
				

			modelBuilder.Entity<Link>()
				.HasRequired(e => e.UserApplication).WithMany(s=>s.Links).WillCascadeOnDelete(true);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<UserApplication> ClientApplications { get; set; }
		public DbSet<Link> Links { get; set; }
		
    }
}