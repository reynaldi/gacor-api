using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GacorAPI.Data.Entities
{
    [Table("Blogs")]
    public class Blog : BaseEntity
    {
        public long CreatorId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual User Creator { get; set; }
    }
}