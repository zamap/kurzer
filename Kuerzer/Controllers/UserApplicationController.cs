using System;
using System.Collections.Generic;
using System.Web.Http;
using Kuerzer.Filters;
using Kuerzer.Helper;
using KuerzerModels;
using KuerzerRepositories.Interfaces;
using WebMatrix.WebData;

namespace Kuerzer.Controllers
{

    public class UserApplicationController : ApiControllerBase
    {
		private readonly ILinkCreator _linkCreator;
		public UserApplicationController(IKuerzerUow uow, ILinkCreator linkCreator)
		{
			Uow =uow;
			_linkCreator = linkCreator;
		}

		[HttpGet]
	
		public UserApplication RegisterApplication(string applicationName, string securetyKey, string email)
		{
			var userProfile = Uow.UserProfiles.GetById(WebSecurity.GetUserId(email));
			if (_linkCreator.GenerateKey(userProfile.SecuretyKey + email) != securetyKey) return null;

			var app = new UserApplication
				{
					UserApplicationId = Guid.NewGuid(),
					Name = applicationName,
					//Links = new List<Link>(),
				};

			if (userProfile.UserApplications == null)
				userProfile.UserApplications = new List<UserApplication>();
			userProfile.UserApplications.Add(app);

			Uow.Commit();
			return app;
		}


		[HttpGet]
		public bool DeleteApplication(Guid appId, string securetyKey, string email)
		{
			//WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
			var userProfile = Uow.UserProfiles.GetById(WebSecurity.GetUserId(email));
			if (_linkCreator.GenerateKey(userProfile.SecuretyKey + email) != securetyKey) return false;
			
			Uow.UserApplications.Delete(Uow.UserApplications.GetById(appId));
	
			Uow.Commit();

			return true;
		}


    }
}
