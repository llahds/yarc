using Microsoft.EntityFrameworkCore;

namespace Api.Data.Entities
{
    [Index(nameof(IsDeleted))]
    [Index(nameof(IsHidden))]
    public class Comment
    {
        public int Id { get; set; }
        public User PostedBy { get; set; }
        public int PostedById { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public Comment Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Comment> Children { get; set; }
        public ICollection<CommentVote> Votes { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ReportedComment> ReportedComments { get; set; }
        public bool IsHidden { get; set; }
    }
}
