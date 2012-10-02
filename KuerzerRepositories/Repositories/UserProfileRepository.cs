using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KuerzerModels;
using KuerzerRepositories.Interfaces;
using KuerzerRepositories.Repositories;

namespace CodeCamper.Data
{
	public class UserProfileRepository : EFRepository<UserProfile>, IUserProfileRepository
	{
		public UserProfileRepository(DbContext context) : base(context) { }

		public IQueryable<UserProfile> GetUserProfiles()
		{

			//var gg = DbSet.Select(s => new UserProfile()
			//{
			//	UserId = s.UserId,
			//	UserName = s.UserName,

			//	SecuretyKey = s.SecuretyKey
			//});

			return DbSet.Select(f=>f);;
		}

	}
}
