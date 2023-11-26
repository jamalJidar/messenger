using ChatRoom.Models;
using ChatRoom.Models.MessageContext;
using ChatRoom.Services.MessagePaaswordService;
using ChatRoom.Services.PersonService;
using ChatRoom.Services.UserSeenService;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;
using System.Reflection;
using System.Text;

namespace ChatRoom.Services.MessageService
{
    public interface IMessageService : ICrudService<Message>
    {

        //public Task<Contact_Message> GetContactAsync(string userName);
        public Task<List<Message>> LsitMessage(string sender, string rec);
		public Task<List<Message>> LsitMessage(Guid recid);
		public Task<List<Message>> LsitMessage(Guid recid, Guid SenderId);
        public Task<List<Message>> LastMessage(Guid recid, Guid SenderId );

        public Task<List<Message>> LsitMessage(Guid recid, Guid SenderId , MessageStatus status);
		public Task<List<Message>> LsitMessage(Guid UserId, MessageStatus status, bool agreeing);
        public Task<KeyValuePair<int, string>> GetCount_LastMessage(Guid recid, Guid SenderId);

    }

    public class MessageService : BaseService<Message>, IMessageService
    {


        private readonly IPersonService personService;
        private readonly IUserSeenService userSeenService;

        private readonly IMessagePaaswordService messagePaaswordService;
		public MessageService(IOptions<MessageDbContext>
            mongoDatabaseSettings, IPersonService _personService, 
            IUserSeenService _userSeenService,
            IMessagePaaswordService 
             _messagePaaswordService
            
            ) : base(mongoDatabaseSettings)
		{
			personService = _personService;
			userSeenService = _userSeenService;
			 messagePaaswordService = _messagePaaswordService;
		}



		public async Task<Message?> GetAsync(Guid id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        //userid
     //   public async Task<Contact_Message> GetContactAsync(string userName)

     //   {
     //       var _C = await _collection.Find(x => x.SenderUserName == userName || x.ReceiverUserName == userName).ToListAsync();



     //       if (_C.Count() > 0)
     //       {
     //           Contact_Message contact_Message = new Contact_Message();
     //           foreach (var item in _C)
     //           {
     //               var receiver = await personService.GetByUserName(item.ReceiverUserName);
     //               var sender = await personService.GetByUserName(item.SenderUserName);
     //               var userSeen = await userSeenService.GetAsync(userName.Trim().ToLower());
     //               var text = await messagePaaswordService.Dcript(item);

					//contact_Message.Message.Add(new MessageViewModel()
     //               {
     //                   body = text.Text  ,

					//      recvId = receiver.Id,
     //                   sender = sender.Id,
     //                   recvIsGroup = false,
     //                   status = 2

     //               });

     //               contact_Message.Contact.Add(new ContactViewModel()
     //               {
     //                   id = receiver.Id,
     //                   name = receiver.UserName,
     //                   number = receiver.PhoneNumber,
     //                   pic = "data:image/jpeg;base64," + Convert.ToBase64String(receiver.ProfileImg, 0, receiver.ProfileImg.Length),
     //                   lastSeen = userSeen.LastSeen,
     //                   owner = receiver.UserName == userName

     //               });

     //           }

     //           return contact_Message;

     //       }

     //       return null;


           
     //   }

        public async Task<KeyValuePair<int, string>> GetCount_LastMessage(Guid recid, Guid SenderId)
        {



            var message = await _collection.Find(x => x.ReceiverId == recid && x.SenderId == SenderId 
            
             ||   x.SenderId == recid && x.ReceiverId == SenderId
            
            ).ToListAsync();


            if (message.Count > 0)
            {
                var lastMessage=
					 await messagePaaswordService.Dcript(message.OrderByDescending(x => x.Time).FirstOrDefault())	;

				
				return new
                   KeyValuePair<int, string>(message.Where(m => m.MessageStatus != MessageStatus.read
                && m.ReceiverId == recid
               ).Count(), lastMessage.Text);

			}


            return new KeyValuePair<int, string>(0, string.Empty);
		 
			 


		}

        public async Task<List<Message>> LsitMessage(Guid recid) =>
         await _collection.Find(x => x.ReceiverId == recid && x.SenderId == recid).ToListAsync();
        public async Task<List<Message>> LsitMessage(string sender, string rec) =>
           await _collection.Find(x => x.SenderUserName == sender && x.ReceiverUserName == rec
           || x.SenderUserName == rec && x.ReceiverUserName == sender

           ).ToListAsync();


        public async Task<List<Message>> LsitMessage(Guid recid, Guid senderId)
        =>  await _collection.Find(x => x.ReceiverId == recid && x.SenderId == senderId).ToListAsync();

        public async Task<List<Message>> LastMessage(Guid recid, Guid senderId)
        => await _collection.Find(x => 
        (x.ReceiverId == recid && x.SenderId == senderId ) 
        ||
        (x.ReceiverId == senderId && x.SenderId == recid) 
        ).SortByDescending(x=>x.Time).ToListAsync();

        



        public async Task<List<Message>> LsitMessage(Guid UserId, MessageStatus status, bool agreeing) =>
         agreeing ? await _collection.Find(x => x.ReceiverId == UserId && x.MessageStatus == status).ToListAsync()
            : await _collection.Find(x => x.ReceiverId == UserId && x.MessageStatus != status).ToListAsync()
            ;

		public async Task<List<Message>> LsitMessage(Guid recid, Guid SenderId, MessageStatus status)=>
		 await _collection.Find(x => x.ReceiverId == recid && x.SenderId == SenderId && x.MessageStatus==status).ToListAsync();


		public async Task RemoveAsync(Guid id) =>
          await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveAsync(List<Guid> id) =>
           await _collection.DeleteManyAsync(x => id.Contains(x.Id));

        public async Task UpdateAsync(Guid id, Message updateModel) =>
             await _collection.ReplaceOneAsync(x => x.Id == id, updateModel);


        public async Task UpdateAsync(List<Message> updateModel)
        {
            foreach (var item in updateModel)
            {
                await UpdateAsync(item.Id, item);
            }

        }
    }
}
