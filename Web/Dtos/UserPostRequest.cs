using System.ComponentModel.DataAnnotations;

namespace Web.Dtos
{
    public class UserPostRequest
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}