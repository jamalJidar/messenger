using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ChatRoom.Models
{

    public enum OnlieUserStatus
    {
        Online, 
        OffLine, 
        Background
    }


	[CollectionName("Person")]
    public class Person : MongoIdentityUser<Guid>
	{

        public bool Status { get; set; }
        public string? Bio { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public byte[] ProfileImg { get; set; } = null!;
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime DateCreate { get; set; }
        public string? FullName { get; set; } = null!;
    }


    public class UserSeen
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime LastSeen { get; set; } = DateTime.Now;
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; } = DateTime.Now;
    }


    public class Contact
    {
        public Guid Id { get; set; }
        /// <summary>
        /// ممکن است یک کاربر در لیست مخاطبین من باشد اما من در لیست مخاطبین او نباشم
        /// مالک
        /// </summary>
        public Guid Owner { get; set; }
    
        /// <summary>
        /// مشترک
        /// </summary>
        public Guid Subscribers { get; set; }

        public bool IsSubscribers { get; set; } = false;

    }

    public class OnlieUser
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserIdentifier { get; set; } = null!;
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Time { get; set; } = DateTime.Now;
        public string OnlineContact { get; set; } = null!;
        public  OnlieUserStatus Status { get; set; }
    
    }



	


	public class ApplicationRole : MongoIdentityRole<Guid> { }






}
