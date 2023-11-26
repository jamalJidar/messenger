using ChatRoom.Hubs;
using ChatRoom.Models;
using ChatRoom.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting.Internal;

namespace ChatRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ChatApiController : ControllerBase
    {
        private IHubContext<ChatHub> hubContext;
        //private Person Person {  get; set; }
        private readonly IHttpContextAccessor httpContext;
		private readonly IWebHostEnvironment _env;

        private IFileService _fileService;
		public ChatApiController(IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContext, IFileService fileService, IWebHostEnvironment env)
		{
			this.hubContext = hubContext;
			this.httpContext = httpContext;
			this._fileService = fileService;
			_env = env;


			// this.hubContext.Clients.User().SendAsync("Init");
		}
		[HttpGet]
        public async Task< ActionResult> Index()
        {

            Console.WriteLine("call api ...");
            ///   await hubContext.Clients.User().SendAsync("Init");
            Console.WriteLine("token : " + await HttpContext.GetTokenAsync("access_token")      +"  ---- " + await HttpContext.GetTokenAsync("Bearer", "access_token"));
            //HttpContext.GetTokenAsync("Bearer", "access_token")
            return Ok("test ...");

        }


		


	}
}
