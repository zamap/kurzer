using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Kuerzer.Helper
{
	public class LinkCreator : ILinkCreator
	{

		private const int BaseNum = 62;
		private const String BaseDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		
		
		private string Base62ToString(long fromValue)
		{
			var toValue = fromValue == 0 ? "0" : "";
			while (fromValue != 0)
			{
				var mod = (int)(fromValue % BaseNum);
				toValue = BaseDigits.Substring(mod, 1) + toValue;
				fromValue = fromValue / BaseNum;
			}

			return toValue;
		}

		public string CreateMD5Hash(string longUrl)
		{
			// Use input string to calculate MD5 hash
			var md5 = MD5.Create();
			byte[] inputBytes = Encoding.ASCII.GetBytes(longUrl);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			// Convert the byte array to hexadecimal string
			var sb = new StringBuilder();
			for (var i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}
			return sb.ToString();
		}

		public string CreateShortcut(string longUrlHash)
		{
			return Base62ToString(BitConverter.ToInt32(Encoding.UTF8.GetBytes(longUrlHash), 0));
		}

		public string GetRedirectDomainName()
		{
			var settings = ConfigurationManager.AppSettings; //"RedirectDomainName");
			var redirectDomainName = settings.Get("RedirectDomainName");
			if (string.IsNullOrEmpty(redirectDomainName))
				throw new Exception("RedirectDomainName is not set in config file.");
			return redirectDomainName;
		}

		public bool CheckLinkStatus(Uri uri)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Timeout = 15000;

			HttpWebResponse response;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception)
			{
				return false; //could not connect to the internet (maybe) 
			}
			return response.StatusCode == HttpStatusCode.OK;
		}


        public string AcquireHTML(string address)
        {
        	HttpWebResponse response = null;
			try
            {
            	var request = WebRequest.Create(address) as HttpWebRequest;
            	if (request != null)
            	{
            		request.UserAgent = "your-search-bot";
            		request.KeepAlive = false;
            		request.Timeout = 10 * 1000;
            		response = request.GetResponse() as HttpWebResponse;

            		if (request.HaveResponse && response != null)
            		{
	           			var reader = new StreamReader(response.GetResponseStream());
            			var sbSource = new StringBuilder(reader.ReadToEnd());
            			response.Close();
            			return sbSource.ToString();
            		}
            		return string.Empty;
            	}
            }
            catch (Exception ex)
            {
            	if (response != null) response.Close();
            	return string.Empty;
            }
			return string.Empty;
        }

		public string GetMetaDescription(string strIn)
		{
			string metaDescription;
			try
			{
				var descriptionMatch = Regex.Match(strIn, "<meta name=\"description\" content=\"([^<]*)\">", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				metaDescription = descriptionMatch.Groups[1].Value;

				//Match KeywordMatch = Regex.Match(strIn, "<meta name=\"keywords\" content=\"([^<]*)\">", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				//_metaKeywords = KeywordMatch.Groups[1].Value;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}

			return metaDescription;
		}

		public string GetMetaTitle(string strIn)
		{
			string metaTitle;
			try
			{
				var titleMatch = Regex.Match(strIn, "<title>([^<]*)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				metaTitle = titleMatch.Groups[1].Value;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}

			return metaTitle;
		}

		public string GenerateKey()
		{
			// Use input string to calculate MD5 hash
			var md5 = MD5.Create();
			byte[] inputBytes = Encoding.ASCII.GetBytes(Guid.NewGuid().ToString());
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			// Convert the byte array to hexadecimal string
			var sb = new StringBuilder();
			for (var i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}

			return sb.ToString();
		}

		public string GenerateKey(string value)
		{
			// Use input string to calculate MD5 hash
			var md5 = MD5.Create();
			byte[] inputBytes = Encoding.ASCII.GetBytes(value);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			// Convert the byte array to hexadecimal string
			var sb = new StringBuilder();
			for (var i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}

			return sb.ToString();
		}

	}
}