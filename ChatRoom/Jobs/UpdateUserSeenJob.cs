using ChatRoom.Models;
using ChatRoom.Services.PersonService;
using ChatRoom.Services.UserSeenService;
using Quartz;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ChatRoom.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.MongoDB;

namespace ChatRoom.Jobs
{
    public class UpdateUserSeenJob :  IJob 
    {
        private readonly IUserSeenService userSeenService;
        private readonly IPersonService personService;
		private UserManager<Person> userManager;



		private string UserName= string.Empty;
		public UpdateUserSeenJob(IUserSeenService _userSeenService, IPersonService personService
,   UserManager<Person> userManager)
		{
			this.userSeenService = _userSeenService;
			this.personService = personService;

			UserName = string.Empty;
			 
			this.userManager = userManager;
		}

		public Task Execute(IJobExecutionContext context)
        {  
            try
            {
				//var username =  userManager.us;
               // Console.WriteLine(username);
			}
            catch (Exception ex)
            {

                Console.WriteLine($"job - {ex.Message} - {  "user is null"} ");
            }



            return Task.FromResult(true);
        }

        private async Task<Person> GetUser() => await personService.GetByUserName(GetUserName());
        //private async Task UpdateSeen(Person user) => await userSeenService.CreateOrUpdateAsync(new UserSeen()
        //{
        //    LastSeen = DateTime.Now,
        //    UserId = user.Id,
        //    UserName = user.UserName

        //});

        private string GetUserName()
        {
            //var userId = accessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            //    accessor?.HttpContext?.User?.Identity?.Name ?? string.Empty

            try
            {
                var islogin = UserName != string.Empty;

               // Console.WriteLine(new Accessor().HttpContext.User.Identity.Name);

               // Console.WriteLine($" username  :{islogin} -  { UserName}   ");
            }
            catch (Exception)
            {

                throw;
            }


            return System.Security.Principal.WindowsIdentity.GetCurrent().Name; 
        }
		 

	}
}
