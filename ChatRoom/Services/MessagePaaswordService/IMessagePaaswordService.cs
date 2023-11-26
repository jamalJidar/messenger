using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.Services.MessageDocumentService;
using ChatRoom.ViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Drawing.Printing;
using System.Reflection;

namespace ChatRoom.Services.MessagePaaswordService
{
	public interface IMessagePaaswordService
	{

		public Task<KeyValuePair<byte[], bool>?> Encript(MessageEncriptViewModel message);
		public Task<MessagDecript?> Dcript(Message model);
		public Task<List<MessagePaasword>?> GetAsync();
		public Task<List<MessagePaasword>?> GetAsync(Guid u1, Guid u2);
		public Task<MessagePaasword?> GetAsync(Guid docId);
		public Task<List<MessagePaasword>?> GetAsync(Guid u1, Guid u2, MessageType type);
		public Task CreateAsync(MessagePaasword model);
		public Task CreateAsync(List<MessagePaasword> models);
		public Task UpdateAsync(Guid id, MessagePaasword updateModel);
		public Task UpdateAsync(List<MessagePaasword> updateModel);
		public Task RemoveAsync(Guid id);
		public Task RemoveAsync(List<Guid> id);
		public DateTime GenerateRandomTime();
	}

	public class MessagePaaswordService : IMessagePaaswordService
	{
		public IMongoCollection<MessagePaasword> _collection;
		private readonly IMessageDocumentService messageDocumentService;
		public MessagePaaswordService(
			IOptions<MessageDbContext> mongoDatabaseSettings, IMessageDocumentService messageDocumentService)
		{
			var mongoClient = new MongoClient(
				mongoDatabaseSettings.Value.ConnectionString);

			var mongoDatabase = mongoClient.GetDatabase(
				mongoDatabaseSettings.Value.Name);
			_collection = mongoDatabase.GetCollection<MessagePaasword>(mongoDatabaseSettings.Value.MessagePaaswordCollectionName);
			this.messageDocumentService = messageDocumentService;
		}
		public static int counter { get; set; } = 0;
		public async Task<List<MessagePaasword>?> GetAsync() =>
				  await _collection
						.Find(_ => true).ToListAsync();
		public async Task<List<MessagePaasword>?> GetAsync(Guid u1, Guid u2) =>
				  await _collection
						.Find(x => (x.User1 == u1 && x.User2 == u2)
								|| (x.User2 == u1 && x.User1 == u2)
							 ).ToListAsync();
		public async Task<List<MessagePaasword>?> GetAsync(Guid u1, Guid u2, MessageType type) =>
				  await _collection
						.Find(x => ((x.User1 == u1 && x.User2 == u2)
								|| (x.User2 == u1 && x.User1 == u2))
								&& x.MessageType == type
						).ToListAsync();
		public async Task<KeyValuePair<byte[], bool>?> Encript(MessageEncriptViewModel message)
		{
			var KeyMessage = await FindKeyMessage(message);
			if (KeyMessage != null)
				return
					  message.MessageType == MessageType.MessageDocument ?
					  new KeyValuePair<byte[], bool>(
					  Utility.AesEncryption.Encrypt(message.bodyByte, KeyMessage.Key, KeyMessage.Iv), status)
					  :
					  new KeyValuePair<byte[], bool>(
					  Utility.AesEncryption.Encrypt(message.body, KeyMessage.Key, KeyMessage.Iv), status)
					  ;

			return null;



		}
		private bool status = false;
		private async Task<MessagePaasword?> FindKeyMessage(MessageEncriptViewModel message)
		{
			status = false;
			var model = await GetAsync(message.sender, message.recvId, message.MessageType);
			var lastKey = new MessagePaasword();
			if (message.MessageType == MessageType.MessageDocument)
				return await newMessagePaasword(message.sender, message.recvId, message.MessageType, message.Id);
			if (model.Count() > 0)
			{
				lastKey = model.OrderByDescending(x => x.To).FirstOrDefault();
				var compareTime = (message.Time - lastKey.To).TotalSeconds;

				if (compareTime > 0)
				{
					status = true;
					return await newMessagePaasword(message.sender, message.recvId, message.MessageType);
				}
				else return lastKey;

			}


			else
			{
				status = true;
				return await newMessagePaasword(message.sender, message.recvId, message.MessageType);
			}


		}


		public async Task<MessagDecript?> Dcript(Message item)
		{


			if (item == null)
				return null;



			var KeyMessageList = await GetAsync(item.ReceiverId, item.SenderId, item.MessageType);
			var messageDocument = await messageDocumentService.GetAsync(item.Id);
			if (KeyMessageList.Count() > 0)
			{

				var KeyMessage = KeyMessageList.Where(x => item.Time >= x.From && item.Time <= x.To).FirstOrDefault();

				Utility.AesEncryption.Decrypt(item.Text, KeyMessage.Key, KeyMessage.Iv, out string decMessage);
				byte[] docByte = null;

				if (messageDocument != null)
				{
					var KeyDoc = await GetAsync(item.Id);
					if (KeyDoc != null)
					{
						Console.WriteLine($" iv : {KeyDoc.Iv.Length}");

						docByte = Utility.AesEncryption.DecryptDoc(messageDocument.Document, KeyDoc.Key, KeyDoc.Iv);
					}

				}
				if (decMessage != string.Empty)
				{
					return new MessagDecript()
					{
						Id = item.Id,
						MessageStatus = item.MessageStatus,
						MessageType = item.MessageType,
						ReceiverId = item.ReceiverId,
						ReceiverUserName = item.ReceiverUserName,
						SenderId = item.SenderId,
						SenderUserName = item.SenderUserName,
						Text = decMessage,
						Time = item.Time,
						PersionTime = Utility.RelativeTimeCalculator.Calculate(item.Time),
						Document = messageDocument != null,
						DocumentFile = messageDocument != null ? Utility.MediaGenerator.Generator(docByte, messageDocument.DocumentType) : string.Empty

					};
				}



			}
			return null;
		}


		private async Task<MessagePaasword> newMessagePaasword(Guid user1, Guid user2, MessageType type, Guid? id = null)
		{
			var conf = Utility.AesEncryption.ConfigEncriptor();
			var mp = new MessagePaasword();
			mp.Id = id == null ? Guid.NewGuid() : id.Value;
			mp.Key = conf.Key;
			mp.Iv = conf.Value;
			mp.User1 = user1;
			mp.User2 = user2;
			mp.From = Utility.RelativeTimeCalculator.SetKind(DateTime.Now, DateTimeKind.Local);
			mp.To = Utility.RelativeTimeCalculator.SetKind(GenerateRandomTime(), DateTimeKind.Local);
			mp.MessageType = type;
			await CreateAsync(mp);
			return mp;
		}

		public async Task CreateAsync(MessagePaasword model) =>
		 await _collection.InsertOneAsync(model);
		public async Task CreateAsync(List<MessagePaasword> models) =>
		await _collection.InsertManyAsync(models);
		public async Task UpdateAsync(Guid id, MessagePaasword updateModel) { }
		public async Task UpdateAsync(List<MessagePaasword> updateModel) { }
		public async Task RemoveAsync(Guid id) { }
		public async Task RemoveAsync(List<Guid> id) { }

		public DateTime GenerateRandomTime()
		{

			int num = Utility.RandomKey.GetRandomNumber(0, 3);

			if (num > 2)
				num = 2;
			switch (num)
			{
				case 0:
					return DateTime.Now.AddDays(Utility.RandomKey.GetRandomNumber(1, 4));

				case 1:
					return DateTime.Now.AddMinutes(Utility.RandomKey.GetRandomNumber(10, 59));
				case 2:
					return DateTime.Now.AddHours(Utility.RandomKey.GetRandomNumber(10, 23));

			}
			return DateTime.Now.AddDays(1);



		}

		public async Task<MessagePaasword?> GetAsync(Guid docId) =>
		   await _collection.Find(x => x.Id == docId).FirstOrDefaultAsync();
	}



}
