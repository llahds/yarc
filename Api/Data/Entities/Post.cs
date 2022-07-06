using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("Posts", Schema = "Social"), Index(nameof(CreatedOn))]
    [Index(nameof(IsHidden))]
    [Index(nameof(IsDeleted))]
    [Index(nameof(Hot))]
    [Index(nameof(Top))]
    [Index(nameof(New))]
    [Index(nameof(Rising))]
    public class Post 
    {
        public int Id { get; set; }
        [MaxLength(400)]
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public Forum Forum { get; set; }
        public int ForumId { get; set; }
        public User PostedBy { get; set; }
        public int PostedById { get; set; }
        public ICollection<ReportedPost> ReportedPosts { get; set; }
        public bool IsHidden { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostVote> Votes { get; set; }
        public bool IsDeleted { get; set; }

        public decimal Hot { get; set; }
        public decimal Top { get; set; }
        public decimal New { get; set; }
        public decimal Rising { get; set; }

        public int CommentCount { get; set; }
        public int Ups { get; set; }
        public int Downs { get; set; }
    }
}
