using System.Linq;
using KuerzerModels;

namespace KuerzerRepositories.Interfaces
{
	public interface IUserProfileRepository : IRepository<UserProfile>
    {
        IQueryable<UserProfile> GetUserProfiles();
    }
}
