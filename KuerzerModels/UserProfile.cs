using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuerzerModels
{
	[Table("UserProfile")]
	public class UserProfile
	{
		//private List<UserApplication> _userApplications;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }
		public string UserName { get; set; }
		
		public string SecuretyKey { get; set; }

		public virtual List<UserApplication> UserApplications { get; set; }
	}
}
