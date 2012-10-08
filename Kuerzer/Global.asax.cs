using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kuerzer.App_Start;
using KuerzerRepositories;

namespace Kuerzer
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	//public class Cf : DefaultControllerFactory
	//{
	//	protected override System.Web.Mvc.IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, System.Type controllerType)
	//	{
	//		return base.GetControllerInstance(requestContext, controllerType);
	//	}

	//	public override IController CreateController(RequestContext requestContext, string controllerName)
	//	{
	//		return base.CreateController(requestContext, controllerName);
	//	}
	//}
	
	public class MvcApplication : HttpApplication
	{
		public static Guid AppId
		{
			get;
			set; //return Guid.Parse("4fb4af4b-c449-496e-9c46-f2bcf97142d6");
		}

		public static string SecKey { get; set; }

		protected void Application_Start()
		{

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			
			//Database.SetInitializer(new KuerzerDatabaseInitializer());
		

			BundleConfig.RegisterBundles(BundleTable.Bundles);

			IocConfig.RegisterIoc(GlobalConfiguration.Configuration);  
			AuthConfig.RegisterAuth();
			
			




		}
	}
}