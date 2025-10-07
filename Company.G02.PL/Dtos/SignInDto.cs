using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class SignInDto
    {

        [Required(ErrorMessage = "Email is requird !!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is requird !!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
