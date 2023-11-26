using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.ViewModels
{
    public class CurrentUser
    {

        public Guid  id { get; set; }
        public string name { get; set; } = null!;
        public string number { get; set; } = null!;
       
        public string pic { get; set; }
       

    }


    public class ContactViewModel:CurrentUser
    {
          public DateTime lastSeen { get; set; }

        public bool owner { get; set; } 



    }
    public class Contact_Message
    {


        public Contact_Message()
        {
            this.Message =new   List<MessageViewModel>();
            this.Contact= new List<ContactViewModel>();
        }

        public List<MessageViewModel> Message { get; set; }
        public List<ContactViewModel> Contact { get; set; }

    }


}
