using System.ComponentModel.DataAnnotations.Schema;

namespace GacorAPI.Data.Entities
{
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        public long CommenterId { get; set; }
        public long BlogId { get; set; }
        public string Body { get; set; }

        public virtual User Commenter { get; set; }
    }
}