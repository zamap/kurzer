using System;
using System.Data.Entity;
using System.Linq;
using KuerzerModels;
using KuerzerRepositories.Interfaces;

namespace KuerzerRepositories.Repositories
{
	public class LinkRepository : EFRepository<Link>, ILinkRepository
	{
		public LinkRepository(DbContext context) : base(context) { }

		public Link GetLinkById(Guid id)
		{
			return DbSet.Select(s => s).FirstOrDefault(s => s.LinkId == id);
		}
	}
}
