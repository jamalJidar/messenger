using ChatRoom.Models;
using ChatRoom.Services;
using ChatRoom.Services.ContactService;
using ChatRoom.Services.GroupMessageService;
using ChatRoom.Services.GroupService;
using ChatRoom.Services.MessageDocumentService;
using ChatRoom.Services.MessagePaaswordService;
using ChatRoom.Services.MessageService;
using ChatRoom.Services.OlineUserService;
using ChatRoom.Services.PersonGroupService;
using ChatRoom.Services.PersonService;
using ChatRoom.Services.UserSeenService;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Servers;
using System.ComponentModel.DataAnnotations;

namespace ChatRoom.Hubs
{
    public class ChatHub : Hub
    {

        private readonly IPersonService personService;
        private readonly IMessageService messageService;
        private readonly IContactService contactService;
        private readonly IUserSeenService userSeenService;
        private IMessagePaaswordService messagePaaswordService;
        private readonly IGroupService groupService;
        private readonly IGroupMessageService groupMessageService;
        private readonly IPersonGroupService personGroupService;
        private readonly IUserOnlineService userOnlineService;
        private readonly IWebHostEnvironment _env;
		#region Property  
		private readonly IFileService _fileService;
        private readonly IMessageDocumentService messageDocument;
        #endregion
		/// <summary>
		/// Definition of models 
		/// </summary>
		private List<Contact> Contacts { get; set; }
        private Person User { get; set; }
        private List<OnlieUser> OnlieUsers { get; set; }
        private string hashCode = string.Empty;

		public ChatHub(IPersonService personService, IMessageService messageService,
			IContactService contactService, IUserSeenService userSeenService,
			IMessagePaaswordService messagePaaswordService,
			IGroupService groupService,
			IPersonGroupService personGroupService,
			IWebHostEnvironment env,
			IGroupMessageService groupMessageService,
			IUserOnlineService userOnlineService, IFileService fileService, IMessageDocumentService messageDocument)
		{
			this.personService = personService;
			this.messageService = messageService;
			this.contactService = contactService;
			this.userSeenService = userSeenService;
			this.messagePaaswordService = messagePaaswordService;
			this.groupService = groupService;
			this.personGroupService = personGroupService;
			_env = env;
			this.groupMessageService = groupMessageService;
			this.userOnlineService = userOnlineService;
			this.Contacts = new List<Contact>();
			this.OnlieUsers = new List<OnlieUser>();
			_fileService = fileService;
			this.messageDocument = messageDocument;
		}
		private static List<KeyValuePair<string, string>> NtoIdMappingTable = new List<KeyValuePair<string, string>>();
        public override async Task OnConnectedAsync()
        {

			this.User=  await GetUser(); ;
			string json = Newtonsoft.Json.JsonConvert.SerializeObject(OnlieUsers);
			var statusUser = await userOnlineService.CheckStatusPersonAsync(User.Id);
            //Console.WriteLine($" status  : {statusUser.Id}");
			if (statusUser != null)
			{

					statusUser.UserIdentifier = Context.ConnectionId;
					statusUser.Time = DateTime.Now;
					statusUser.OnlineContact = json;
					statusUser.Status = OnlieUserStatus.Online;
					await userOnlineService.UpdateAsync(statusUser.UserId, statusUser);
				


			}
			else
			{
				await userOnlineService.CreateAsync(new OnlieUser()
				{
					OnlineContact = json,
					Status = OnlieUserStatus.Online,
					Time = DateTime.Now,
					UserId = User.Id,
					UserIdentifier = Context.ConnectionId,
					UserName = Context?.User?.Identity?.Name
				});
			}

			await base.OnConnectedAsync();


        }
        public async Task Init()
        {
            await FillCurrentUser();
            await FillListContact();
          //  await UserOnlineStatus();
            await ListContact();
            var messages = await messageService.LsitMessage(User.Id, MessageStatus.read, false);
            if (messages.Count() > 0)
                messages.ForEach(x => x.MessageStatus = MessageStatus.delivered);
            await messageService.UpdateAsync(messages);


        }
        private async Task FillCurrentUser() => this.User = await GetUser();
        private async Task<Person> GetUser(string username = "") =>
          username.Length > 3 && string.IsNullOrWhiteSpace(username) ?
                await personService.GetByUserName(username) :
                await personService.GetByUserName(Context.User.Identity.Name.Trim().ToLower());
        private async Task FillListContact() =>
        this.Contacts = await contactService.GetListAsync(User.Id);
        public async Task UserOnlineStatus()
        {
			var user = await GetUser(); ;
			string json = Newtonsoft.Json.JsonConvert.SerializeObject(OnlieUsers);
			var statusUser = await userOnlineService.CheckStatusPersonAsync(user.Id);
			if (statusUser != null)
			{

				if (statusUser.Status != OnlieUserStatus.Online)
				{
					statusUser.UserIdentifier = Context.ConnectionId;
					statusUser.Time = DateTime.Now;
					statusUser.OnlineContact = json;
					statusUser.Status = OnlieUserStatus.Online;
					await userOnlineService.UpdateAsync(statusUser.Id, statusUser);
				}
			}
			else
			{
				await userOnlineService.CreateAsync(new OnlieUser()
				{
					OnlineContact = json,
					Status = OnlieUserStatus.Online,
					Time = DateTime.Now,
					UserId = user.Id,
					UserIdentifier = Context.ConnectionId,
					UserName = Context?.User?.Identity?.Name
				});
			}


		}
		//request as sender to reciver
		public async Task RequestOnlineUserAsSenderToReciver(string reciverId)
        {
            if (Contacts.Count() > 0)
            {
                var temp = await userOnlineService.CheckStatusPersonAsync(Contacts.Select(x => x.Subscribers).ToList());
                await Clients.User(reciverId).SendAsync("RequestOnlineUserAsSenderToReciver", User.Id);
                //temp.Select(x => x.UserIdentifier).ToList()

                //this.OnlieUsers.Except(await userOnlineService.CheckStatusPersonAsync(Contacts.Select(x => x.Subscribers).ToList())).ToList();
            }
        }
        //response from reciver
        public async Task ResponseOnlineUserReciver(Guid senderId, Guid receverId)
        {
            var _sId = await userOnlineService.CheckStatusPersonAsync(senderId);
            //this.OnlieUsers.Add(await userOnlineService.GetAsync(receverId));
            await Clients.User(senderId.ToString()).SendAsync("RequestOnlineUserAsReciverToSender", receverId);

        }
        public async Task ResponseOnlineUserAsReciverToSender(Guid receverId)
        {
            //Console.WriteLine($"online Users :  {OnlieUsers.Count()}");
        }
        private async Task DisconnectAsync()
        {
            Console.WriteLine("dis ..... ");
            var user = await GetUser();
            var statusUser = await userOnlineService.CheckStatusPersonAsync(user.Id);
            if (statusUser != null)
            {


                statusUser.Status = OnlieUserStatus.OffLine;
                statusUser.OnlineContact = string.Empty;
                statusUser.UserIdentifier = string.Empty;
                await userOnlineService.UpdateAsync(statusUser.UserId, statusUser);
            }

            //Console.WriteLine($"DisconnectAsync {User.UserName}");
        }

        //private async Task FillListOnlineUsers() =>
        //	OnlieUsers = await userOnlineService.CheckStatusPersonAsync(this.Contacts.Select(x => x.Subscribers).ToList());

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await DisconnectAsync();
            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserId(string receiver)
        {

            var temp = userOnlineService.CheckStatusPersonAsync(receiver).Result;
            if (temp != null)
                return temp.UserIdentifier;
            return string.Empty;

        }
        public Task JoinRoom(string roomName) => Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        public async Task Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text.Trim())) return;
            var user = await GetPersonAsync();
            await Clients.User(GetUserId(user.UserName)).SendAsync("Search", await personService.SearchByUserName(text));
        }

        public async Task MessageDocument(byte[] file, Guid messageId)
        {

        }
        public async Task sendMessage(Guid reciverId, string message ,	Guid?	_messageId)
        {
       
            var messageId = _messageId.HasValue ? _messageId.Value : Guid.NewGuid();
 
            this.User =await GetUser();
            //Get details of the recipient of the message
            Person _u = await personService.GetAsync(reciverId);
            if (_u != null)
            {

                //Get the details of the user who sent the message
                
                var RECIVER = await userOnlineService.GetAsync(reciverId);
                  
                DateTime time = DateTime.Now;
                //getting encrypted text message
                    var messageEncript =
                    await messagePaaswordService.Encript(
                    new MessageEncriptViewModel()
                    {
                        body = message,
                        recvId = _u.Id,
                        sender = User.Id,
                        Time = time,
                        MessageType = MessageType.Message,

						Status = false,
                        Id = messageId
                    });
                var _message = new Message()
                {
                    Id = messageId,
                    MessageType = MessageType.Message,
                    ReceiverUserName = _u.UserName,
                    SenderUserName = User.UserName,
                    Text = messageEncript.Value.Key,
                    Time = messageEncript.Value.Value ? DateTime.Now : time,
                    MessageStatus = RECIVER != null ? MessageStatus.delivered : MessageStatus.send,
                    ReceiverId = _u.Id,
                    SenderId = User.Id, 
                };
                await messageService.CreateAsync(_message);
                var mdoc =await messageDocument.GetAsync(messageId);
               

				MessagDecript? messagDecript = await messagePaaswordService.Dcript(await messageService.GetAsync(messageId));
                if(messagDecript!=null)
					messagDecript.Document= mdoc!=null;
				Console.WriteLine($"RECIVER!=null   :  {RECIVER!=null}");
				if (RECIVER!=null)
                {		Console.WriteLine($"RECIVER STATUS :  {RECIVER.Status}");
                    if(RECIVER.Status== OnlieUserStatus.Online )
                    {
                         await newContact(RECIVER.UserId.ToString(), _u.Id);
                    await Clients.User(RECIVER.UserId.ToString()).SendAsync("ReceiveMessage", messagDecript);
                  
                    await Clients.User(RECIVER.UserId.ToString()).SendAsync("CheckOpenChatPrivateUser", messagDecript);
                    }
                    else
                    {
						await Clients.User(User.Id.ToString()).SendAsync("ReceiveMessage", messagDecript);
					}
                }
                else
                { await Clients.User(User.Id.ToString()).SendAsync("ReceiveMessage", messagDecript); }
                var reciverContact = await contactService.GeSingleAsync(_u.Id, User.Id);
                if (reciverContact == null)
                { await contactService.CreateAsync(new Contact() { Owner = _u.Id, Subscribers = User.Id, IsSubscribers = false }); }
            }
        }
         public async Task CheckOpenChatPrivateUser(MessagDecript message, bool status)
        {
            Console.WriteLine($"message encript :", message.Text);

			var statusUser = await userOnlineService.GetAsync(message.ReceiverId);
		 
			if (statusUser.Status == OnlieUserStatus.Online)
            {   
                  
                   message.MessageStatus =   status == true ? MessageStatus.read : MessageStatus.delivered;

                var encMessage = await messageService.GetAsync(message.Id);
                 encMessage.MessageStatus = status == true ? MessageStatus.read : message.MessageStatus;
                await messageService.UpdateAsync(message.Id, encMessage);
                Console.WriteLine(encMessage.MessageStatus);
                if (message.MessageStatus == MessageStatus.delivered) { await ListContact(message.ReceiverUserName); }
                await Clients.User(message.SenderId.ToString()).SendAsync("ReceiveMessage", message);
            }
        }
		#region Upload  
		//[HttpPost(nameof(Upload))]
		public async Task Upload( List<IFormFile> formFiles)
		{

			string subDirectory = "/video";
			try
			{
				_fileService.UploadFile(formFiles, subDirectory);


			Console.WriteLine($"count  :  {formFiles.Count}  size :  { _fileService.SizeConverter(formFiles.Sum(f => f.Length))}" );
			}
			catch (Exception ex)
			{
				   Console.WriteLine(ex.Message);
			}

		}
		#endregion
		private async Task newContact(string reciver, Guid resId)
        {
            var contact = await contactService.GetListAsync(resId);
            var conatcUser = await personService.GetAsync(contact.Select(x => x.Subscribers).ToList());
            conatcUser = conatcUser.Where(c => c.Id != resId).ToList();
        }
        public async Task ListContact(string username = "")
        {
            var _u = await GetUser(username);
            //var contact = await contactService.GetListAsync(_u.Id);
            this.OnlieUsers.Clear();


            if (Contacts.Count() > 0)
            {

               /// = await userOnlineService.CheckStatusPersonAsync(Contacts.Select(x => x.Owner).ToList());
                var userConatctList = await personService.GetAsync(Contacts.Select(x => x.Subscribers).ToList());
                if (userConatctList.Count() > 0)
                {
                    userConatctList = userConatctList.Where(c => c.Id != _u.Id).ToList();
                    List<ListContactViewModel> listContacts = new List<ListContactViewModel>();
                    foreach (var item in userConatctList)
                    {
                        var _lastSeen =await userOnlineService.CheckStatusPersonAsync(item.Id);
                        this.OnlieUsers.Add(_lastSeen);
                        var _messages = await messageService.LastMessage(recid: _u.Id, SenderId: item.Id);
                        var messageTextDecript = new KeyValuePair<int, string>(0, "");
                          DateTime? timelastmessage = null;
                        if (_messages.Count > 0)
                        {
                            timelastmessage = _messages.OrderByDescending(x => x.Time).FirstOrDefault().Time;
                            messageTextDecript = new KeyValuePair<int, string>(
                            _messages.Where(x => x.MessageStatus != MessageStatus.read && x.ReceiverId == _u.Id)
                        .Count(),
                           messagePaaswordService.Dcript(_messages.OrderByDescending(x => x.Time).FirstOrDefault()).Result.Text);
                        }

                        listContacts.Add(new ListContactViewModel()
                        {
                            userName = item.UserName,
                            phoneNumber = item.PhoneNumber,
                            Count_LastMessage = messageTextDecript,
                            Id = item.Id,
                            isOnline =
                            _lastSeen != null ?
                              (_lastSeen.Status == OnlieUserStatus.Online ? true: false):false 
                            ,
                            profileImage = "data:image/jpeg;base64," + Convert.ToBase64String(item.ProfileImg, 0, item.ProfileImg.Length),
                            lastSeen = _lastSeen!=null?  
                              (_lastSeen.Status==OnlieUserStatus.Online ? "آنلاین" : Utility.RelativeTimeCalculator.Calculate(_lastSeen.Time)) :
                            "گذشته",
                            
                            Name = item.UserName,
                            MessageType = MessageType.Message , 
                            Time= (timelastmessage.HasValue ? timelastmessage.Value: null)
                        });
                    }

                    var GroupPerson = await personGroupService.ListGroup(_u.Id);
                    var ListGroup = await groupService.ListGroup(GroupPerson.Select(x => x.GroupId).ToList());
                    if (ListGroup.Count() > 0)
                    {
                        foreach (var pg in ListGroup)
                        {
                            byte[] prof = System.IO.File.ReadAllBytes($"{_env.WebRootPath}/images/group.jpg");
                            listContacts.Add(new ListContactViewModel()
                            {
                                Id = pg.Id,
                                isOnline = false,
                                MessageType = MessageType.Group,
                                Name = pg.Name,
                                Count_LastMessage = new KeyValuePair<int, string>(0, ""),
                                profileImage = "data:image/jpeg;base64," + Convert.ToBase64String(prof, 0, prof.Length),
                                lastSeen = "",
                                phoneNumber = pg.Name,
                                userName = pg.Name
                            });
                            await JoinRoom(pg.Id.ToString());
                        }
                    }

                    listContacts = listContacts.OrderByDescending(x => x.Time).ToList();
                    await Clients.Caller.SendAsync("ListContact", listContacts);
                }
            }
        }
        public async Task sendMessageG(string group, string message)
        {
            if (string.IsNullOrWhiteSpace(message.Trim()) || string.IsNullOrWhiteSpace(group.Trim()))
                return;
            var user = await GetUser();
            var _g = await groupService.FindByName(group);
            if (_g == null)
                return;
            var userGroup = await personGroupService.FindGroup(user.Id, _g.Id);
            if (userGroup != null)
            {
                var encMessage = await messagePaaswordService.Encript(new MessageEncriptViewModel()
                {
                    body = message,
                    MessageType = MessageType.Group,
                    recvId = _g.Id,
                    sender = user.Id,
                    Status = true,
                    Time = DateTime.Now
                });
                await groupMessageService.CreateAsync(new GroupMessage()
                {
                    GroupId = userGroup.GroupId,
                    SenderId = user.Id,
                    MessageType = MessageType.Group,
                    Text = encMessage.Value.Key,
                    Time = DateTime.Now
                });

            }
            await Clients.User(GetUserId(user.UserName)).SendAsync("ReceiveMessage", message, false);
        }

        public async Task addNewGroup(string groupName)
        {
            var GroupId = Guid.NewGuid();
            PersonGroup personGroup = new PersonGroup()
            { GroupId = GroupId, PersonId = User.Id, Type = GroupMemberType.Creator };
            await personGroupService.CreateAsync(personGroup);
            Group groupMessage = new Group()
            {
                Id = GroupId,
                Time = DateTime.Now,
                Name = groupName,
                Description = "Description",
                ProfileImg = System.IO.File.ReadAllBytes($"{_env.WebRootPath}/images/group.jpg")
            };
            await groupService.CreateAsync(groupMessage);
            await Clients.Caller.SendAsync("addNewGroup", "گروه ایجاد شد");
        }
        public async Task UserProfile()
        {
            if (User == null)
                User = await personService.GetByUserName(Context?.User?.Identity.Name);
            await Clients.Caller.SendAsync("UserProfile",
            new CurrentUser()
            {
                id = User.Id,
                name = User.UserName.Trim(),
                number = User.PhoneNumber,
                pic = "data:image/jpeg;base64," + Convert.ToBase64String(User.ProfileImg, 0, User.ProfileImg.Length)
            });
        }
        private async Task<Person> GetPersonAsync() => await personService.GetByUserName(Context.User.Identity.Name.Trim().ToLower());
        public async Task GetPicProfile() => await Clients.Caller.SendAsync("GetPicProfile", new FileContentResult(User.ProfileImg, "image/jpeg"));
        //todo => convert fies to stream  
        public async Task LsitMessage(Guid recid, Guid caller)
        {
            var user = await personService.GetAsync(recid);
            var currnetUser = await GetUser();
            //list send  message current user ....
            var _messages = await messageService.LsitMessage(recid: user.Id, SenderId: currnetUser.Id);
            //list get message current user
            var temp = await messageService.LsitMessage(recid: currnetUser.Id, SenderId: recid);
            if (temp.Count() > 0)
            {
                temp.Where(x => x.ReceiverId == currnetUser.Id && x.MessageStatus != MessageStatus.read)
                    .ToList().ForEach(m =>
                    {
                        m.MessageStatus = MessageStatus.read;
                    });
                await messageService.UpdateAsync(temp);
                _messages.AddRange(temp);
            }
            var messagDecriptList = new List<MessagDecript>();

            if (_messages.Count() > 0)
            {
                foreach (var item in _messages)
                {
                    try
                    {
                        var messageDecriptItem = await messagePaaswordService.Dcript(item);

                        if (messageDecriptItem != null)
                            messagDecriptList.Add(messageDecriptItem);
                            }
                    catch
                    { }
                }
			    	//await Clients.User(currnetUser.Id.ToString())
						  //.SendAsync("ReceiveMessage", messagDecriptList);
			}

            await Clients.User(caller.ToString()).SendAsync("LsitMessage", messagDecriptList.OrderBy(x => x.Time).ToList(), caller);
            var messageDecript = new KeyValuePair<int, string>(0, "");
            string txt = messagDecriptList?.OrderByDescending(x => x.Time)?.FirstOrDefault() != null ?
                messagDecriptList?.OrderByDescending(x => x.Time)?.FirstOrDefault().Text : string.Empty;
            if (temp.Count() > 0)
                messageDecript = new KeyValuePair<int, string>(temp.Where(x => x.MessageStatus != MessageStatus.read).Count(), txt);
            await ListContact();
            bool isContact = false;
            var contact = await contactService.GeSingleAsync(user.Id, currnetUser.Id);

            if (contact != null)
                if (contact.IsSubscribers) isContact = true;


            var lastSeen = await userOnlineService.GetAsync(user.Id);
            await Clients.User(caller.ToString()).SendAsync("ShowProfileOtherUser",
                

                new ListContactViewModel()
                {
                    Id = user.Id,
                    isOnline = (lastSeen != null)  ? 
                    lastSeen.Status==OnlieUserStatus.Online?true:false
                    : false,
                    lastSeen =
                    (lastSeen != null) ?
                    lastSeen.Status == OnlieUserStatus.Online ? "آنلاین" :
                    Utility.RelativeTimeCalculator.Calculate(lastSeen.Time)
                    : ""
                    ,
                    userName = isContact ? user.FullName : user.UserName,
                    profileImage = "data:image/jpeg;base64," + Convert.ToBase64String(user.ProfileImg, 0, user.ProfileImg.Length),
                    phoneNumber = user.FullName,
                    Name = user.UserName,
                    Count_LastMessage = messageDecript
                }
); ;
        }
        public async Task LsitMessageGruop(Guid guid)
        {
            var currnetUser = await GetUser();

            //list send  message current user ....
            var tempGroup = await groupService.GetAsync(guid);
            var _messages = await groupService.LsitMessageGroup(guid);
            var imgByte = System.IO.File.ReadAllBytes($"{_env.WebRootPath}/images/group.jpg");
            var _group = new ViewModelGroup()
            {
                Description = tempGroup.Description,
                Id = tempGroup.Id,
                Name = tempGroup.Name,
                ProfileImg = "data:image/jpeg;base64," + Convert.ToBase64String(imgByte, 0, imgByte.Length)
            };
            if (_messages.Count() > 0 && _messages != null)
            {
                var messagDecriptList = new List<MessagDecript>();

                foreach (var item in _messages)
                {
                    var _message = new Message()
                    {
                        Id = item.Id,
                        Time = item.Time,
                        MessageStatus = MessageStatus.send,
                        MessageType = MessageType.Group,
                        ReceiverId = _group.Id,
                        SenderId = item.SenderId,
                        SenderUserName = currnetUser.UserName,
                        Text = item.Text,

                    };
                    var decript = await messagePaaswordService.Dcript(_message);
                    messagDecriptList.Add(new MessagDecript()
                    {
                        Id = item.Id,
                        MessageStatus = MessageStatus.delivered,
                        MessageType = MessageType.Group,
                        ReceiverId = item.GroupId,
                        SenderId = item.SenderId,
                        Text = decript.Text,
                        SenderUserName = currnetUser.FullName,
                        PersionTime = Utility.RelativeTimeCalculator.Calculate(item.Time),
                        Time = item.Time
                    });
                }
                await Clients.User(GetUserId(currnetUser.UserName)).SendAsync("LsitMessageGroup",
               _group, messagDecriptList.OrderBy(x => x.Time).ToList());
            }
            else await Clients.User(GetUserId(currnetUser.UserName)).SendAsync("LsitMessageGroup", _group, null);
        }
        public async Task UserType(Guid userId, bool type)
        {
            var _u = await personService.GetAsync(userId);
            var currntUser = await GetUser();
            string RECIVER = _u != null ? GetUserId(_u.UserName) : string.Empty;
            await Clients.User(RECIVER).SendAsync("UserType", currntUser.Id, type);
        }

    }
}
