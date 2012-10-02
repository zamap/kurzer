using System;
using System.Linq;
using KuerzerModels;

namespace KuerzerRepositories.Interfaces
{
	public interface IUserApplicationRepository : IRepository<UserApplication>
	{
		UserApplication GetById(Guid id);
	}
}
