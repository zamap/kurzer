using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using KuerzerCommon;
using Kuerzer.Controllers;
using KuerzerModels;
using KuerzerRepositories.Interfaces;
using WebMatrix.WebData;

namespace KuerzerApi.Controllers
{
	
	public class UserProfileController : ApiControllerBase
	{
		private readonly ILinkCreator _linkCreator;
		public UserProfileController(IKuerzerUow uow, ILinkCreator linkCreator)
		{
			Uow = uow;
			_linkCreator = linkCreator;
		}

		// GET api/userprofile
		public IEnumerable<UserProfile> Get()
		{
			//var userProfiles = Uow.UserProfiles.GetUserProfiles();
			//if (userProfiles != null) return userProfiles;
			return Uow.UserProfiles.GetAll();
		}

		[HttpGet]
		//http://localhost:58516/api/UserProfile/ConfirmAccount?confirmation=Iq0WW6v3_spR6SHCxWgbVg2&email=ostetic@gmail.com
		public bool ConfirmAccount(string confirmation, string email)
		{
			//if user did not confirm registration 
			if (WebSecurity.IsConfirmed(email)) return false;

			if (!WebSecurity.ConfirmAccount(confirmation)) return  false;
			
			var userProfile = Uow.UserProfiles.GetById(WebSecurity.GetUserId(email));
			userProfile.SecuretyKey = _linkCreator.GenerateKey();
			Uow.Commit();

			return SendSecuretyKeyMessage(email, userProfile.SecuretyKey);
		}

		// DELETE api/userprofile/5
		//http://localhost:58516//api/UserProfile/RegisterUser?email=ostetic@gmail.com&pwd=tr!atata
		[HttpGet]
		public bool RegisterUser(string email, string pwd)
		{
			
			if (WebSecurity.UserExists(email)) return false;
			if (!(IsValidPassword(pwd)) && !(IsValidEmailAddress(email))) return false;
			
			var	confirmationToken = WebSecurity.CreateUserAndAccount(email, pwd, null, true);
			

			//todo: add queue
			//send regisration confirmation request

			return SendConfirmationAccountMessage(email, confirmationToken); ;
		}


		private bool SendConfirmationAccountMessage(string clientEmail, string confirmationToken)
		{
			try
			{
				var confirmationUrl = "http:/localhost:58516/api/UserProfile/ConfirmAccount?confirmation=" +
									  HttpUtility.UrlEncode(confirmationToken) + "&email=" + clientEmail;
				var confirmationLink = String.Format("<a href=\"{0}\">Click to confirm your registration</a>", confirmationUrl);
				
				var emailBodyText =
					"<p>Thank you for signing up with us! Please confirm your registration by clicking the following link:</p>"
					+ "<p>" + confirmationLink + "</p>"
					+ "<p>In case you need it, here's the confirmation code:<strong> " + confirmationToken + "</strong></p>";
					//"<p>There is you securetyKey: <strong>" + securetyKey + " </strong> Please use it like md5(securetyKey + <url>) <p>";

				WebMail.Send(clientEmail, "account confirmation", emailBodyText);
				
				return true;
			}
			catch(Exception ex )
			{
				return false;
			}
			
		}


		private bool SendSecuretyKeyMessage(string email, string securetyKey )
		{
			try
			{
				var emailBodyText =
				"<p>Thank you for confirm registration</p>"
				+ "<p>In case you need it, here's the security key:<strong> " + securetyKey + "</strong></p>" +
				"<p> you can use it like md5(securetyKey + <url>) <p>";

				WebMail.Send(email, "account confirmation", emailBodyText);

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}


		private bool IsValidEmailAddress(string address)
		{
			if (string.IsNullOrEmpty(address))
				return false;
			else
			{
				var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
				return regex.IsMatch(address) && !address.EndsWith(".");
			}
		}


		static bool IsValidPassword(string password)
		{
			const int MIN_LENGTH = 6;
			const int MAX_LENGTH = 15;

			if (password == null) return false;

			var meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
			var hasUpperCaseLetter = false;
			var hasLowerCaseLetter = false;
			var hasDecimalDigit = false;

			if (meetsLengthRequirements)
			{
				foreach (var c in password)
				{
					if (char.IsUpper(c)) hasUpperCaseLetter = true;
					else if (char.IsLower(c)) hasLowerCaseLetter = true;
					else if (char.IsDigit(c)) hasDecimalDigit = true;
				}
			}
			return meetsLengthRequirements && hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit;

		}

		
	}
}
