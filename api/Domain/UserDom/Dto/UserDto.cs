using System.ComponentModel.DataAnnotations;

namespace GacorAPI.Domain.UserDom.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Field email is required"), EmailAddress(ErrorMessage = "Should enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field first name is required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        /// Should hash password to sha256
        /// </summary>
        [Required(ErrorMessage = "Field password is required")]
        public string Password { get; set; }        
    }
}