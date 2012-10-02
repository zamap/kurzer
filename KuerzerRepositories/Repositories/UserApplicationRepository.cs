using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using KuerzerModels;
using KuerzerRepositories.Interfaces;

namespace KuerzerRepositories.Repositories
{
	public class UserApplicationRepository : EFRepository<UserApplication>, IUserApplicationRepository
	{
		public UserApplicationRepository(DbContext context) : base(context)
		{

		}

		public UserApplication GetById(Guid id)
		{
			//return DbSet.FirstOrDefault(PredicateBuilder.GetByIdPredicate<T>(id));
			return DbSet.Find(id);
		}

		//private string GenerateApplicationKey()
		//{
		//	// Use input string to calculate MD5 hash
		//	var md5 = MD5.Create();
		//	byte[] inputBytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
		//	byte[] hashBytes = md5.ComputeHash(inputBytes);

		//	// Convert the byte array to hexadecimal string
		//	var sb = new StringBuilder();
		//	for (var i = 0; i < hashBytes.Length; i++)
		//	{
		//		sb.Append(hashBytes[i].ToString("X2"));
		//	}
		//	return sb.ToString();
		//}



		//private bool SendConfirmationRequest(string clientEmail)
		//{

		//	if (!IsValidEmailAddress(clientEmail)) return false;

		//	{
		//	var mailMsg = new MailMessage();
		//	mailMsg.To.Add(clientEmail);
		//	// From
		//	var mailAddress = new MailAddress("you@hotmail.com");
		//	mailMsg.From = mailAddress;

		//	// Subject and Body
		//	mailMsg.Subject = "registration confirmation request";
		//		mailMsg.IsBodyHtml = true;
		//	mailMsg.Body = "body";

		//	// Init SmtpClient and send on port 587 in my case. (Usual=port25)
		//	var smtpClient = new SmtpClient("mailserver", Convert.ToInt32("587"));
		//	var credentials = new System.Net.NetworkCredential("username", "password");
		//	smtpClient.Credentials = credentials;

		//	smtpClient.Send(mailMsg);

		//	return true;
		//}

		//private bool IsValidEmailAddress(string address)
		//{
		//	if (string.IsNullOrEmpty(address))
		//		return false;
		//	else
		//	{
		//		var regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
		//		return regex.IsMatch(address) && !address.EndsWith(".");
		//	}

		//}

	}
}
