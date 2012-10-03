using System;
using System.Linq;
using KuerzerModels;

namespace KuerzerRepositories.Interfaces
{
	public interface ILinkRepository : IRepository<Link>
    {
		Link GetLinkById(Guid id);
    }
}
