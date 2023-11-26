using ChatRoom.Models;
using ChatRoom.Services.MessagePaaswordService;
using ChatRoom.Services.MessageService;
using ChatRoom.Services.PersonService;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Controllers
{
	public class Accessor : Controller
	{

		private readonly IPersonService personService;
		private readonly IMessagePaaswordService messagePaaswordService;
		private readonly IMessageService messageService;
		public Accessor(IMessagePaaswordService messagePaaswordService, IPersonService personService, IMessageService messageService)
		{
			this.messagePaaswordService = messagePaaswordService;
			this.personService = personService;
			this.messageService = messageService;
		}

		public async Task<IActionResult> Index1()
		{

			var user1 = await personService.GetByUserName("jamaljaj");
			var user2 = await personService.GetByUserName("test");
			var messageId = Guid.NewGuid();



			var messageText = await messagePaaswordService.Encript(new ViewModels.MessageEncriptViewModel()
			{
				body = Services.PhoneNumberGeneratorService.TextRandomGenerator.Generate(),
				recvId = user1.Id,
				sender = user2.Id,
				Time = DateTime.Now

			});


			var message = new Message()
			{
				Id = messageId,
				MessageStatus = 0,
				MessageType = MessageType.Message,
				ReceiverUserName = "jamaljaj",
				SenderUserName = "test",
				Time = DateTime.Now,
				ReceiverId = user1.Id,
				SenderId = user2.Id,
				Text = messageText.Value.Key
			};


			await messageService.CreateAsync(message);

			var messPass = await messagePaaswordService.GetAsync(user1.Id, user2.Id);

			var listMessage = new List<MessagDecript>();

			foreach (var item in await messageService.GetAsync())
			{
				var _ = await messagePaaswordService.Dcript(item);
				listMessage.Add(_);
			}



			return Json(
				new
				{
					message = await messageService.GetAsync(),
					listMessage,
					messPass

				}
				);


			return Content("");
		}


		public async Task<IActionResult> Index()
		{
			for (int i = 0; i < 4; i++)
			{


			}
			var From = DateTime.Now;
			var To = DateTime.Now.AddSeconds(4);
			var time = DateTime.Now.AddSeconds(3);

			return Content((time.Ticks >= From.Ticks && time.Ticks <= To.Ticks).ToString());
		}

		public async Task<IActionResult> Index2()
		{
			List<Message> messageList = new List<Message>();
			List<MessagDecript> decript = new List<MessagDecript>();
			var Receiver = await personService.GetByUserName("test");
			var Sender = await personService.GetByUserName("jamaljaj");
			for (int i = 0; i < 3; i++)
			{
				var Id = Guid.NewGuid();
				string text = ChatRoom.Services.PhoneNumberGeneratorService.TextRandomGenerator.Generate(); //"salam";
				var time = Utility.RelativeTimeCalculator.SetKind(DateTime.Now, DateTimeKind.Local);
				var _t = await messagePaaswordService.Encript(
				new MessageEncriptViewModel()
				{ body = text, recvId = Receiver.Id, Id = Id, sender = Sender.Id, Time = time, });
				var message = new Message()
				{
					Id = Id,
					MessageStatus = MessageStatus.send,
					ReceiverId = Receiver.Id,
					SenderId = Sender.Id,
					ReceiverUserName = Receiver.UserName,
					MessageType = 0,
					SenderUserName = Sender.UserName,
					Text = _t.Value.Key,
					Time = time

				};

				messageList.Add(message);
				await messageService.CreateAsync(message);
				Thread.Sleep(2000);
			}
			var _messageList = await messageService.LsitMessage(Receiver.Id, Sender.Id);
			_messageList.AddRange(await messageService.LsitMessage(Sender.Id, Receiver.Id));
            
			foreach (var item in _messageList)
			{
				var messageDecript = await messagePaaswordService.Dcript(item);
				if (messageDecript != null)
				{
					decript.Add(messageDecript);
				}

			}

			var messPass =await messagePaaswordService.GetAsync();
			
			return Json(new { messageList, messPass, decript  });
		}

	}
}
