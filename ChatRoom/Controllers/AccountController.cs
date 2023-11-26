using ChatRoom.Models;
using ChatRoom.Services;
using ChatRoom.Services.FileConverter;
using ChatRoom.Services.PersonService;
using ChatRoom.Services.PhoneNumberGeneratorService;
using ChatRoom.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;



namespace ChatRoom.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<Person> userManager;
        private SignInManager<Person> signInManager;
        private readonly IPersonService _currentUser;
        private readonly IFileConverterService fileConverterService;
        private readonly IWebHostEnvironment env;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IPersonService personService;



        //private readonly IAuthService _authService;
        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager,
            IPersonService currentUser, IFileConverterService fileConverterService,
            IWebHostEnvironment env, RoleManager<ApplicationRole> roleManager,
            IPersonService personService
            //IAuthService authService
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _currentUser = currentUser;
            this.fileConverterService = fileConverterService;
            this.env = env;
            this.roleManager = roleManager;
            this.personService = personService;
            //_authService = authService;
        }

        [HttpGet("AddRole")]
        public async Task<IActionResult> AddRole()
        {
            var users = await personService.GetAsync();

            foreach (var item in users)
            {

                if (await roleManager.RoleExistsAsync("User"))
                    await userManager.AddToRoleAsync(item, "User");

            }

            return Json(
                new
                {
                    data = users
                });


        }


        [Route("Login")]
        public async Task<IActionResult> Login()
        {

            //Person appUser = await personService.GetByPhoneNumber("09117953544");

            //await signInManager.PasswordSignInAsync(appUser, "a123456", false, false);
            //return Redirect("/Home/Index");

            return View();
        }
        [Route("Login")]
        [HttpPost()]
        [AllowAnonymous]

        // public async Task<IActionResult> Login([Required][EmailAddress] string email, [Required] string password, string returnurl)
        public async Task<IActionResult> Login(LoginUser loginUser)
        {

            if (ModelState.IsValid)
            {
                Person appUser = await personService.GetByPhoneNumber(loginUser.PhoneNumber);

                //  Console.WriteLine(appUser.FullName);

                if (appUser != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(appUser, loginUser.Password, false, false);
                    if (result.Succeeded)
                    {

                        Console.WriteLine(User.Identity.Name);
                        return Redirect("/Home/Index");
                    }

                    else
                    {

                        return Json(new { result });
                    }
                }
                Console.WriteLine("error");
                //ModelState.AddModelError(nameof(loginUser.PhoneNumber), "خطا در ورود!!!!!");
                //	return View();

            }

            string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
            Console.WriteLine(messages);

            return View();

            //if (!ModelState.IsValid)
            //             return BadRequest("Invalid payload");
            //         var res = await _authService.Login(loginUser);

            //         if (res.Status != true)
            //             return BadRequest(res);
            //         return Json(res);
            //         try
            //{

            //}
            //catch (Exception ex)
            //{

            //	return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + " sadsadsadsadasd");
            //}
            //return View();


        }
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(RegisterUser register)
        {
            if (ModelState.IsValid)
            {
                //  _roleManager.CreateAsync(new ApplicationRole() { Name = "admin" });

                var uu = await _currentUser.GetByUserName(register.UserName);
                if (uu != null)
                {
                    return Json(new
                    {
                        Message = "نام کاربری وارد شده تکراری می باشد",
                        StatusCode = 402
                    });
                }

                byte[] pic = null;
                if (register.ProfileImg != null)
                {

                    pic = await fileConverterService.Convert(register.ProfileImg);
                }
                var _p = new Person()
                {
                    Status = true,
                    PhoneNumber = register.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,

                    DateCreate = DateTime.Now,
                    UserName = register.UserName,
                    ProfileImg = pic,
                    FullName = register.Name,
                    Bio = register.Bio,
                    // Roles = _roleManager.Roles.Select(x => x.Id).ToList()
                };

                //_p.AddRole()

                var resUser = await userManager.CreateAsync(_p, register.Password);

                if (!resUser.Succeeded)
                {
                    string xx = string.Empty;
                    resUser.Errors.ToList().ForEach(x =>
                    {
                        xx += $"{x.Description}- {x.Code}{Environment.NewLine}";
                    });
                    return Json(

                        new { xx }
                        );
                }


                var findUser = await _currentUser.GetByUserName(register.UserName);

                if (findUser == null)
                {
                    return Content("user not found !!!!!");
                }




                return Json(new
                {
                    UserName = register.UserName,
                    Password = register.Password
                });
            }

            return View(register);
        }

        public async Task<IActionResult> newUser()
        {
            List<string> u = new List<string>();
            for (int i = 1; i < 13; i++)
            {

                byte[] profByte = System.IO.File.ReadAllBytes(env.WebRootPath + $"/images/{i}.jpg");
                try
                {
                    string UserName = UserNameGenerator.GenerateRandomUserName();
                    var uu = await _currentUser.GetByUserName(UserName);
                    if (uu == null)
                    {
                        var _p = new Person()
                        {
                            Status = true,
                            PhoneNumber = PhoneNumberGenerator.GenerateRandomPhoneNumber(),
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,

                            DateCreate = DateTime.Now,
                            UserName = UserName,
                            ProfileImg = profByte
                            // Roles = _roleManager.Roles.Select(x => x.Id).ToList() , 
                            ,
                            Bio = "this is test message",
                            FullName = TextRandomGenerator.RandomName(),
                        };

                        var result = await userManager.CreateAsync(_p, "a123456");
                        if (result.Succeeded)
                        {
                            u.Add(UserName);
                        }

                        Console.Write(result.Succeeded);
                    }
                    else
                    {
                        Console.Write("user is exists ...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }




            return Json(new
            {
                u = u
            });
        }


        [Authorize]
        //[HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> ChangePassword(string phone)
        {

            var user = await personService.GetByPhoneNumber(phone);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, "a123456");
            }
            return Redirect("/login");

        }
    }
}
