using System.Web.Http;
using Kuerzer.Helper;
using KuerzerRepositories;
using KuerzerRepositories.Helpers;
using KuerzerRepositories.Interfaces;
using Ninject;
using Ninject.Web.Mvc;

namespace Kuerzer.App_Start
{
	public class IocConfig
	{
		public static void RegisterIoc(HttpConfiguration config)
		{
			var kernel = new StandardKernel(); // Ninject IoC

			// These registrations are "per instance request".
			// See http://blog.bobcravens.com/2010/03/ninject-life-cycle-management-or-scoping/

			kernel.Bind<RepositoryFactories>().To<RepositoryFactories>()
				.InSingletonScope();

			kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
			kernel.Bind<IKuerzerUow>().To<KuerzerUow>();
			kernel.Bind<ILinkCreator>().To<LinkCreator>();

			// Tell WebApi how to use our Ninject IoC
			config.DependencyResolver = new NinjectDependencyResolver(kernel);
		}
	}
}