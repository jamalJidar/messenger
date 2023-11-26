using ChatRoom.Services;
using ChatRoom.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoom.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{

		private readonly IAuthService _authService;
		private readonly ILogger<AuthenticationController> _logger;

		public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
		{
			_authService = authService;
			_logger = logger;
		}


		[HttpGet]
		public async Task<IActionResult> Login(LoginUser model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest("Invalid payload");
				var res = await _authService.Login(model);
				if (res.Status != true)
					return BadRequest(res);
				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, "erorr");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Register(RegisterUser model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest("Invalid payload");
				var (status, message) = await _authService.Registeration(model, UserRoles.Admin);
				if (status == 0)
				{
					return BadRequest(message);
				}
				return CreatedAtAction(nameof(Register), model);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}

}
