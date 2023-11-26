using ChatRoom.Models;
using ChatRoom.Services.ContactService;
using ChatRoom.Services.MessageService;
using ChatRoom.Services.OlineUserService;
using ChatRoom.Services.PersonService;
using ChatRoom.Services.UserSeenService;
using ChatRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Security;

namespace ChatRoom.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{

		private readonly IPersonService personService;
		private readonly IMessageService messageService;
		private readonly IUserSeenService userSeenService;
		private readonly IContactService contactService;
		private readonly IUserOnlineService userOnlineService;
        private readonly IWebHostEnvironment _env;
        public HomeController(ILogger<HomeController> logger,
            IPersonService personService, IMessageService messageService,
            IUserSeenService userSeenService, IContactService contactService, IUserOnlineService userOnlineService, IWebHostEnvironment env)
        {

            this.personService = personService;
            this.messageService = messageService;
            this.userSeenService = userSeenService;
            this.contactService = contactService;
            this.userOnlineService = userOnlineService;
            _env = env;
        }

        public async Task<IActionResult> Index()
		{

			//SpeechSynthesizer
			await lastSeen();
			var _lastseen = await userSeenService.GetAsync(User.Identity.Name);
			//ViewBag.time = _lastseen.Time;

			 personService.GetAsync().Result.ForEach(async x =>
			{
				int random = Utility.RandomKey.GetRandomNumber(100, 10000);
			
				//await	userSeenService.CreateOrUpdateAsync(x.Id, x.UserName , DateTime.Now.AddMinutes(random));


			}); 
			return View();
		}

		public IActionResult Privacy(string txt)
		{
			return Content($"{txt} - salam");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}



		public async Task<IActionResult> Users()
		{
			var _u = await personService.GetAsync();
			return Json(new { users = _u.Select(x => x.UserName) });
		}

		public async Task<IActionResult> SaveContct()
		{

			var users = await personService.GetAsync();
			var currentUser = await personService.GetByUserName(User.Identity.Name);

			var contact = await contactService.GetListAsync(currentUser.Id);
			var conatcUser = await personService.GetAsync(contact.Select(x => x.Subscribers).ToList());
			ViewBag.Subscribers = users;
			ViewBag.contact = conatcUser;
			ViewBag.fullNme = currentUser.FullName;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SaveContct(ViewModels.SaveContactViewModel model)
		{
			var users = await personService.GetAsync();
			var currentUser = await personService.GetByUserName(User.Identity.Name);

			var contact = await contactService.GetListAsync(currentUser.Id);
			var conatcUser = await personService.GetAsync(contact.Select(x => x.Subscribers).ToList());
			ViewBag.Subscribers = users;
			ViewBag.contact = conatcUser;
			if (ModelState.IsValid)
			{


				await contactService.CreateAsync(new Models.Contact()
				{

					Owner = currentUser.Id,
					Subscribers = model.Subscribers,
					IsSubscribers = true
				});

				ViewBag.Subscribers = users.Where(x => x.UserName.Trim().ToLower() != User.Identity.Name.Trim().ToLower()).ToList();

				return View();

			}

			return View(model);
		}

		public async Task<IActionResult> GetPicProfile()
		{
			var _u = await personService.GetByUserName(User.Identity.Name);

			return File(_u.ProfileImg, "image/jpeg");

		}

		public async Task lastSeen()
		{

			//Console.Clear();
			var _u = await personService.GetByUserName(User.Identity.Name);

			//var userSeen = new UserSeen()
			//{

			//    Id=Guid.NewGuid(),
			//    LastSeen = DateTime.Now,
			//    UserName = User.Identity.Name.Trim().ToLower()  , 
			//    UserId= _u.Id 
			//};

			await userSeenService.CreateOrUpdateAsync(_u.Id, _u.UserName);

			//
			//  ($"home :  {userSeen.LastSeen}");
		}

        public  IActionResult  UploadFile()
		{
            Console.WriteLine("test get  !");
            return View();
		}
        [HttpPost]
        public async Task<string> UploadFile([FromForm] IFormFile file)
        {
			Console.Clear();
			Console.WriteLine("test post ....  !");
		
            string path = Path.Combine(_env.WebRootPath, "video/" + file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/Images/" + file.FileName;
        }
    }
}