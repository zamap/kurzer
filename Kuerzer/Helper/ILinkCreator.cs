using System;

namespace Kuerzer.Helper
{
	public interface ILinkCreator
	{
		string CreateMD5Hash(string longUrl);
		string CreateShortcut(string longUrlHash);
		string GetRedirectDomainName();
		bool CheckLinkStatus(Uri uri);

		string AcquireHTML(string address);
		string GetMetaDescription(string strIn);
		string GetMetaTitle(string strIn);

		string GenerateKey(string value);
		string GenerateKey();
		
	}
}