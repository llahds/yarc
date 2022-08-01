using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("Users", Schema = "Social")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    [Index(nameof(IsDeleted))]
    public class User 
    {
        public int Id { get; set; }
        [MaxLength(75)]
        public string DisplayName { get; set; } = "";
        [MaxLength(4000)]
        public string About { get; set; } = "";
        [MaxLength(125)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<ForumOwner> ForumOwners { get; set; }
        public ICollection<ForumModerator> ForumModerators { get; set; }
        public ICollection<Post> Posts { get; set; }
        [MaxLength(35)]
        public string UserName { get; set; }
        public ICollection<ReportedPost> ReportedPosts { get; set; }
        public ICollection<ForumMember> Forums { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostVote> PostVotes { get; set; }
        public ICollection<CommentVote> CommentVotes { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ReportedComment> ReportedComments { get; set; }
        public ICollection<UserSetting> UserSettings { get; set; }
    }
}
