using System.ComponentModel.DataAnnotations;

namespace Jevstafjev.Anecdotes.Web.Pages.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        public string ReturnUrl { get; set; } = null!;
    }
}
