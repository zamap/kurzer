using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace KuerzerModels
{
	public class Link
	{

		//[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid LinkId { get; set; }

		[Required]
		[RegularExpression(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", ErrorMessage = "Please enter a valid URL")]
		public string LongUrl { get; set; }

		public string ShortUrl { get; set; }

		[MaxLength(32)]
		public string LongUrlHash { get; set; }

		/// <summary>
		///returns true if the link is broken 
		/// </summary>
		[DefaultValue(false)]
		public bool IsBroken { get; set; }

		[MaxLength(1000)]
		[DefaultValue("")]
		[DataType(DataType.MultilineText)]
		//[DisplayName("Enter a short description of the link!")]
		public string Description { get; set; }



		[MaxLength(1000)]
		[DefaultValue("")]
		//[DisplayName("Give a title!")]
		public string Title { get; set; }


		[DefaultValue(0)]
		[DisplayName("With group attended a link?")]
		public int Group { get; set; }

		public DateTime? Created { get; set; }

		//public Guid UserApplicationId { get; set; }
		[IgnoreDataMember]
		public virtual UserApplication UserApplication { get; set; }
	}
}
