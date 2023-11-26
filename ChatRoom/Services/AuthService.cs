using ChatRoom.Models;
using ChatRoom.Services.PersonService;
using ChatRoom.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatRoom.Services
{
	public interface IAuthService
	{
		Task<(int, string)> Registeration(RegisterUser model, string role);
		Task<LoginResualt> Login(LoginUser model);
	}
	public class AuthService : IAuthService
	{
		private readonly UserManager<Person> userManager;
		private readonly RoleManager<ApplicationRole> roleManager;
		private readonly IConfiguration _configuration;
		private readonly IPersonService personService;
        private SignInManager<Person> signInManager;
        public AuthService(UserManager<Person> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IPersonService personService, SignInManager<Person> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            this.personService = personService;
            this.signInManager = signInManager;
        }
        public async Task<(int, string)> Registeration(RegisterUser model, string role)
		{
			var userExists = await userManager.FindByNameAsync(model.UserName);
			if (userExists != null)
				return (0, "User already exists");

			Person user = new()
			{
				//	Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName,

			};
			var createUserResult = await userManager.CreateAsync(user, model.Password);
			if (!createUserResult.Succeeded)
				return (0, "User creation failed! Please check user details and try again.");

			if (!await roleManager.RoleExistsAsync(role))
			{
				var _role = new ApplicationRole();
				_role.Name = role;
				await roleManager.CreateAsync(_role);


			}

			if (await roleManager.RoleExistsAsync(role))
				await userManager.AddToRoleAsync(user, role);

			return (1, "User created successfully!");
		}

		public async Task<LoginResualt> Login(LoginUser model)
		{

			Person user = await personService.GetByPhoneNumber(model.PhoneNumber);
			if (user == null)
			return	new LoginResualt()
				{
					ExpiryDate= DateTime.Now,
					Status=false, StatusCode= 400, Token=string.Empty, 
					Message= "user not found"
                };

            if (userManager.SupportsUserLockout && await userManager.IsLockedOutAsync(user))
               return new LoginResualt()
                {
                    ExpiryDate = DateTime.Now,
                    Status = false,
                    StatusCode = 401,
                    Token = string.Empty,
                    Message = "your account for 3 hours is blocked"
                };

          
            if (await userManager.CheckPasswordAsync(user, model.Password))
            {
                if (userManager.SupportsUserLockout &&await userManager.GetAccessFailedCountAsync(user) > 0)
                {
                  await  userManager.ResetAccessFailedCountAsync(user);
                }


                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var ExpiryDate = DateTime.Now.AddHours(1);
                string token = GenerateToken(authClaims, ExpiryDate);
                await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                return new LoginResualt()
                {
                    ExpiryDate = ExpiryDate,
                    Status = true,
                    StatusCode = 200,
                    Token = token,
                    Message = "success"
                };





            }
            else
            {
                if (userManager.SupportsUserLockout &&await userManager.GetLockoutEnabledAsync(user))
                {
                  await  userManager.AccessFailedAsync(user);
                }

               return new LoginResualt()
                {
                    ExpiryDate = DateTime.Now,
                    Status = false,
                    StatusCode = 401,
                    Token = string.Empty,
                    Message = "Invalid username or password"
               };
            }


          

        }


		private string GenerateToken(IEnumerable<Claim> claims , DateTime   ExpiryDate  )
		{
			
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
			//var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = _configuration["JWTKey:ValidIssuer"],
				Audience = _configuration["JWTKey:ValidAudience"],
				//Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
				Expires = ExpiryDate,
				SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
				Subject = new ClaimsIdentity(claims)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

	}

}
