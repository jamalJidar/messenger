using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ChatRoom.Models
{  /// <summary>
   /// class any message = >  per to per , group , document message
   /// </summary>
   /// 

	public enum MessageStatus
	{
		send = 0,
		delivered = 1,
		read = 2,
		sending = 3

	}

	public enum DeleteMessageStatus
	{
		all,
		one

	}
	public class Message
	{
		[Key]
		public Guid Id { get; set; }
		public byte[] Text { get; set; } = null!;
		public string? SenderUserName { get; set; } = null!;
		public Guid SenderId { get; set; }
		public MessageType MessageType { get; set; }
		public string? ReceiverUserName { get; set; } = null!;
		public Guid ReceiverId { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; }
		public MessageStatus MessageStatus { get; set; }
		public DeleteMessageStatus DeleteMessageStatus { get; set; }
		public DateTime _time
		{
			get
			{
				return Utility.RelativeTimeCalculator.SetKind(this.Time, DateTimeKind.Local);
			}
		}






	}
	public class Group
	{
		[Key]
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public byte[] ProfileImg { get; set; } = null!;
		public virtual ICollection<PersonGroup> PersonGroups { get; set; } = null!;
		public virtual ICollection<GroupMessage> Messages { get; set; } = null!;
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; }
		public string Name { get; set; } = null!;

	}


	public class GroupMessage
	{
		[Key]
		public Guid Id { get; set; }
		public Guid GroupId { get; set; }
		public Group Group { get; set; } = null!;
		public byte[] Text { get; set; } = null!;
		public Guid SenderId { get; set; }

		public Person Sender { get; set; } = null!;

		public MessageType MessageType { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; }
	}



	public class PersonGroup
	{
		public Guid Id { get; set; }
		public Guid PersonId { get; set; }
		public Guid GroupId { get; set; }
		public GroupMemberType Type { get; set; }
	}


	public class MessageDocument
	{
		[Key]
		public Guid Id { get; set; }
		public byte[] Document { get; set; } = null!;
		public Guid id { get; set; }
		public Guid MessageId { get; set; }
		public DocumentType DocumentType { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; }
		public DateTime _time
		{
			get
			{
				return Utility.RelativeTimeCalculator.SetKind(this.Time, DateTimeKind.Local);
			}
		}


	}
	public class MessagePaasword
	{

		public Guid Id { get; set; }
		//public Guid MessageId { get; set; } 
		public Guid User1 { get; set; }
		public Guid User2 { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime From { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime To { get; set; }
		public byte[] Key { get; set; } = null!;
		public byte[] Iv { get; set; } = null!;

		public MessagePaaswordType MessagePaaswordType { get; set; } = MessagePaaswordType.AES;

		public MessageType MessageType { get; set; }

		public DateTime _to
		{
			get
			{
				return Utility.RelativeTimeCalculator.SetKind(this.To, DateTimeKind.Local);
			}
		}
		public DateTime _from
		{
			get { return Utility.RelativeTimeCalculator.SetKind(this.From, DateTimeKind.Local); }
		}

		/// <summary>
		/// false => normal message
		/// true => Group Message
		/// </summary>
		public bool Type { get; set; }
	}
	public class UserDownloadDocument
	{ 
	 //DownloadedBits
	 // بیت های دانلود شده از فایل را در حود ذخیره می کند 
	 //فایل در حال دانلود ابتدا در کوکی ذخیره می شود 
	 // اگر دانلود تکیمل بود کوکی خالی میشود و 
	 // LocalAddress
	 // آدرس فایل رو به صورت محلی ذخیزه می کند 
	 // براساس 
	 //ip 
	 // مشخص می شود کاریر فایل را ذخیره کرده یا نه 
	 //اگر
	 //Complete
	 // true  
	 //بود یعنی فایل دانلودش تکمیل شده
	 // پس ابتدا این متغیر چک می شود
	 //بعد آدرس محلی فایل
	 //
		public Guid id { get; set; }
		public Guid UserId { get; set; }
		public Guid DocumentId { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; }
		public IPAddress Ip { get; set; } = null!;
		public byte[] DownloadedBits { get; set; } = null!;
		public bool Complete { get; set; } = false;
		public string LocalAddress { get; set; } = null!;

	}
	public enum DocumentType
	{
		[Display(Name = "ویدئو")]
		Video,
		[Display(Name = "صوتی")]
		Mp3,
		[Display(Name = "pdf")]
		Pdf,
		[Display(Name = "تصویر")]
		Image,
		[Display(Name = "ناشناخته")]
		Unknown,
		[Display(Name = "فایل فشرده")]
		Zip,
		Rar,
		[Display(Name = "فایل متنی")]
		Text,
	}
	public enum MessageType
	{

		Message,
		Group,
		MessageDocument,

	}

	public enum GroupMemberType
	{

		SuperAdmin = 0,
		Admin,
		NormalUser,
		Creator
	}

	public enum MessagePaaswordType
	{
		AES,


	}



}
