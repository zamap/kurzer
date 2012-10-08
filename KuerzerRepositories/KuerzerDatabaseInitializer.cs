using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Web.Security;
using KuerzerModels;
using WebMatrix.WebData;


namespace KuerzerRepositories
{
	public class KuerzerDatabaseInitializer :
		//CreateDatabaseIfNotExists<CodeCamperDbContext>      // when model is stable
		//DropCreateDatabaseIfModelChanges<KuerzerDbContext> // when iterating
		DropCreateDatabaseAlways<KuerzerDbContext>
	{
		protected override void Seed(KuerzerDbContext context)
		{
			
			SeedMembership(context);
			//context.Links.Add(new Link { LinkId = Guid.NewGuid(), Created = DateTime.Now, Description = "aaa", Group = 0, IsBroken = false, LongUrl = "https://bitly.com/", LongUrlHash = "asdadd", ShortUrl = "https://bitly.com/asdasdasd",Title = "titile",User = new UserProfile{UserId = 1,UserName = "asdasdasd"}});
			//context.SaveChanges();
			//MembershipCreateStatus Status;
			//Membership.CreateUser("Demo", "123456", "demo@demo.com", null, null, true, out Status);
			//Roles.CreateRole("Admin");
			//Roles.AddUserToRole("Demo", "Admin");
			//var userProofile = new UserProfile() { UserName = "Petr" };
			//context.UserProfiles.Add(userProofile);
			//context.SaveChanges();

			base.Seed(context);
			//context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX LongUrlHash_Index ON dbo.Links (LongUrlHash)");
			

			//	((IObjectContextAdapter) context).ObjectContext.CreateDatabase();

			//WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
		}

		static void SeedMembership(KuerzerDbContext context)
		{

			WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);

			var roles = (SimpleRoleProvider)Roles.Provider;
			var membership = (SimpleMembershipProvider)Membership.Provider;

			if (!roles.RoleExists("Admin"))
			{
				roles.CreateRole("Admin");
			}
			if (membership.GetUser("user1", false) == null)
			{
				membership.CreateUserAndAccount("user1", "doremifasol");

				var t = context.UserProfiles;
				var g = t.Find(membership.GetUserId("user1"));
				g.SecuretyKey = "12345678901234567890";
				g.UserApplications = new List<UserApplication>(){
																new UserApplication() 
																	{
																		Name = "testappame",
																		UserApplicationId = Guid.Parse("4fb4af4b-c449-496e-9c46-f2bcf97142d6"),
																		Links = new List<Link>()
																			{
																				new Link()
																					{
																						LinkId = Guid.NewGuid(),
																						LongUrl = "http://lenta.ru/",
																						LongUrlHash = "83E8412A10FB18EADD36C716B27395EC",
																						IsBroken = false,
																						Group = 0,
																						Created = DateTime.Now,
																						Description = "Decrtiptiontratata",
																						ShortUrl = "http://go/11tAvA"
																					}
																			}
																	}
																};
				//context.SaveChanges();
				
			}
			//if (!roles.GetRolesForUser("sallen").Contains("Admin"))
			{
				roles.AddUsersToRoles(new[] { "user1" }, new[] { "admin" });
			}
			
		}
	}
}