using System.ComponentModel.DataAnnotations;

namespace ChatRoom.ViewModels.Users
{
	public static class UserRoles
	{
		public const string Admin = "Admin";
		public const string User = "User";
	}
	public class LoginUser
    {

        [Display(Name = "نام کاربری")]
        
        [Required(ErrorMessage = "لطفا {0} را وارد فرمایید")]
        public string PhoneNumber { get; set; } = null!;

        //[DataType(dataType: DataType.Password)]
        [Display(Name = "رمز ورود")]
        [Required(ErrorMessage = "لطفا {0} را وارد فرمایید")]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }="a123456";
        //[Display(Name = "مرا به خاطر بسپار")]
        //[Required(ErrorMessage = "لطفا {0} را وارد فرمایید")]
        //public bool RememberMe { get; set; }





    }


    public class LoginResualt
    {
        public int StatusCode { get; set; } 
        public string Token { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }

        public bool Status { get; set; }
        public string Message { get; set; } = null!;


    }
}
