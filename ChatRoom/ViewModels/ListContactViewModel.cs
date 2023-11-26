using ChatRoom.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatRoom.ViewModels
{
    public class ListContactViewModel
    {

        public string phoneNumber { get; set; } = null!;
        public string userName { get; set; }
        public Guid Id { get; set; }
        public string profileImage { get; set; } = null!;
        public KeyValuePair<int, string> Count_LastMessage { get; set; } = new KeyValuePair<int, string>(0, "");
        public bool  isOnline { get; set; }

        public string lastSeen { get; set; }= null!;

        public string Name { get; set; }= null!;
        public MessageType   MessageType { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
       public DateTime? Time { get; set; }

    }


    /*
     
     contact.Where(c=>c.Subscribers== x.Id).FirstOrDefault().IsSubscribers 
                          ?   x.UserName : x.PhoneNumber

                   , x.PhoneNumber
                   , x.Id ,
                   "data:image/jpeg;base64,"+ Convert.ToBase64String(x.ProfileImg, 0, x.ProfileImg.Length)
                    , messageService.GetCount_LastMessage(recid: _u.Id ,SenderId: x.Id ).Result,
                       GetUserId(x.UserName)!=string.Empty ?true:false 
     
     */
}
