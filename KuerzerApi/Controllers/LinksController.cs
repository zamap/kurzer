using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using KuerzerCommon;
using Kuerzer.Controllers;
using KuerzerModels;
using KuerzerRepositories.Interfaces;

namespace KuerzerApi.Controllers
{
    public class LinksController : ApiControllerBase
    {
	    private readonly ILinkCreator _linkCreator;
		public  LinksController(IKuerzerUow uow, ILinkCreator linkCreator)
		{
			Uow =uow;
			_linkCreator = linkCreator;
		}

	    // GET api/link
        public IEnumerable<Link> Get(Guid appId, string secKey)
        {
			
	        return Uow.Links.GetAll();
        }

        // PUT api/link/5
		public void Put(Link link)
        {
			
			link.LongUrlHash = _linkCreator.CreateMD5Hash(link.LongUrl);


			var g = Uow.Links.GetLinkById(link.LinkId);
			if (g!=null)
			{
				g.Title = link.Title;
			}

			//checking if exist
			if (Uow.Links.GetAll().Any(item => item.LongUrlHash == link.LongUrlHash))
			{
				// Existing entity
				link.IsBroken = !_linkCreator.CheckLinkStatus(new Uri(link.LongUrl));
				link.Created = DateTime.Now;
			
				//_context.Entry(link).State = EntityState.Modified;
			}
			else
			{
				// New entity
				link.LinkId = Guid.NewGuid();
				link.ShortUrl = string.Format("{0}/{1}", _linkCreator.GetRedirectDomainName(),
				                              _linkCreator.CreateShortcut(link.LongUrlHash));
				link.Created = DateTime.Now;
				string info = _linkCreator.AcquireHTML(link.LongUrl);
				link.Description = _linkCreator.GetMetaDescription(info);
				link.Title = _linkCreator.GetMetaTitle(info);

				Uow.Links.Add(link);
			}
			Uow.Commit();
        }
		
		bool CheckSecuretyKey(UserApplication userApplication, string secKey, string value)
		{
			var userProfiles = Uow.UserProfiles.GetUserProfiles();
			UserProfile userProfile =null;
			foreach (var profile in userProfiles)
			{
				if( profile.UserApplications != null && profile.UserApplications.Exists(g => g == userApplication))
					 userProfile = profile;
			}
		
			if (userProfile == null) return false;
			
			return _linkCreator.GenerateKey(userProfile.SecuretyKey + value) == secKey;
		}


	    // PUT api/link/5
		[HttpGet]
	    public Link InsertLink(Guid appId, string secKey, string url)
		{	
			var userApplication= Uow.UserApplications.GetById(appId);
			if (userApplication == null) return null;

			if (!CheckSecuretyKey(userApplication, secKey, url)) return null;

			var longUrlHash = _linkCreator.CreateMD5Hash(url);

			//checking if exist
			if (Uow.Links.GetAll().Any(item => item.LongUrlHash == longUrlHash)) return null;

			// New entity
			var info = _linkCreator.AcquireHTML(url);
			
			var link = new Link()
				{
					LinkId = Guid.NewGuid(),
					ShortUrl = string.Format("{0}/{1}", _linkCreator.GetRedirectDomainName(),_linkCreator.CreateShortcut(longUrlHash)),
					Created = DateTime.Now,
					Description = _linkCreator.GetMetaDescription(info),
					Title = _linkCreator.GetMetaTitle(info),
					LongUrl = url,
					LongUrlHash = longUrlHash,
					Group = 0,
				};
			
			if (userApplication.Links == null)
				userApplication.Links = new List<Link>();
			userApplication.Links.Add(link);
			
			Uow.Commit();
			return link;
		}

        // DELETE api/link/5
		[HttpGet]
		public bool DeleteLink(Guid appId, string secKey, string linkId)
        {
			var userApplication= Uow.UserApplications.GetById(appId);
			if (userApplication == null) return false;

			if (!CheckSecuretyKey(userApplication, secKey, linkId)) return false;
			var link = Uow.Links.GetLinkById(Guid.Parse(linkId));
			Uow.Links.Delete(link);
			Uow.Commit();
	        return true;
        }

		// DELETE api/link/5
		public bool Delete(Link link)
		{

			//var g = Uow.Links.GetById(link.LinkId);
			//Uow.Links.Delete(g);
			//Uow.Commit();
			return true;
		}
    }
}
