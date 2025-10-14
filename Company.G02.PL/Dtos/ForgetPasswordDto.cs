using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.Dtos
{
    public class ForgetPasswordDto
    {

        [Required(ErrorMessage = "Email is requird !!")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
