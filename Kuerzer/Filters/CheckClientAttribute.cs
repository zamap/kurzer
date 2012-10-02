using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using KuerzerRepositories;
using WebMatrix.WebData;
using Kuerzer.Models;

namespace Kuerzer.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class CheckClientAttribute : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
		
					{
				//		if (!context.Database.Exists())
				//		{
				//			// Create the SimpleMembership database without Entity Framework migration schema
				//			((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
				//		}
					}
		}
	}
}
