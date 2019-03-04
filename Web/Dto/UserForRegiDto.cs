using System.ComponentModel.DataAnnotations;

namespace Web.Dto
{
    public class UserForRegiDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }        
    }
}