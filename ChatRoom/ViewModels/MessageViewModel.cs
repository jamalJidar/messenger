using ChatRoom.Models;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.ViewModels
{
	public class MessageViewModel
	{
		public Guid Id { get; set; }
		public Guid sender { get; set; }
		public string body { get; set; } = null!;
		public int status { get; set; }
		public Guid recvId { get; set; }
		public bool recvIsGroup { get; set; }


	}

	public class MessagDecript
	{
		public Guid Id { get; set; }
		public string Text { get; set; } = null!;
		public string SenderUserName { get; set; } = null!;
		public Guid SenderId { get; set; }
		public MessageType MessageType { get; set; }
		public string ReceiverUserName { get; set; } = null!;
		public Guid ReceiverId { get; set; }
		public DateTime Time { get; set; }
		public MessageStatus MessageStatus { get; set; }

        public string PersionTime   { get; set; }= null!;
        public bool Document { get; set; }

		public string DocumentFile { get; set; } = null!;

    }

	public class MessageEncriptViewModel
	{
		public Guid Id { get; set; }
		public Guid sender { get; set; }
		public string body { get; set; } = null!;
		public Guid recvId { get; set; }

		public DateTime Time { get; set; }

        public bool Status { get; set; }

		public MessageType MessageType { get; set; } = MessageType.Message;
        public byte[] bodyByte { get; set; }
    }


}
