using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GacorAPI.Data.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }

        public IList<Blog> Blogs { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}