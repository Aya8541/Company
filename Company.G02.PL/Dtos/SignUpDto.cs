using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage ="UserName is requird !!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is requird !!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is requird !!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is requird !!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is requird !!")]
        [DataType(DataType.Password)]   
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is requird !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password dose Not Match the Password  !!")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
