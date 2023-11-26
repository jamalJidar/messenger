
using System.ComponentModel.DataAnnotations;


namespace ChatRoom.ViewModels.Users
{
    public class RegisterUser
    {

        [Display(Name = "نام کاربری")]
        [Required]
        [MaxLength(length: 24, ErrorMessage = "{0}  نمی تواند بیشتر از {1} حرف باشد")]
        [MinLength(length: 4, ErrorMessage = "{0} نمی تواند کمتر از {1} حرف  باشد")]
        public string UserName { get; set; } = null!;
        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        [Display(Name = "تلفن همراه")]
        [DataType(dataType: DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;


        [Display(Name = "رمز ورود")]
        [Required(ErrorMessage = "{0} الزامی ")]
        public string Password { get; set; } = null!;

        [Display(Name = "تصویر پروفایل")]
        public IFormFile ProfileImg { get; set; }

        public string Name { get; set; }
        public string Bio { get; set; }




    }

  public class RegisterUser1
    {

        [Display(Name = "نام کاربری")]
        [Required]
        [MaxLength(length: 24, ErrorMessage = "{0}  نمی تواند بیشتر از {1} حرف باشد")]
        [MinLength(length: 4, ErrorMessage = "{0} نمی تواند کمتر از {1} حرف  باشد")]
        public string UserName { get; set; } = null!;
        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        [Display(Name = "تلفن همراه")]
        [DataType(dataType: DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;


        [Display(Name = "رمز ورود")]
        [Required(ErrorMessage = "{0} الزامی ")]
        public string Password { get; set; } = null!;

        [Display(Name = "تصویر پروفایل")]
        public Byte[] ProfileImg { get; set; }

    }


}
