using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace KuerzerModels
{
	public class UserApplication
	{
		//private List<Link> _links;

		[Key]
		public Guid UserApplicationId { get; set; }

		[MaxLength(100)]
		[DefaultValue("")]
		public string Name { get; set; }

		public virtual List<Link> Links { get; set; }
		
		
		//public Guid UserProfileId { get; set; }
		[IgnoreDataMember]
		public virtual UserProfile UserProfile { get; set; }
	}
}
