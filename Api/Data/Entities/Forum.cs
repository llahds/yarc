using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("Forums", Schema = "Social")]
    [Index(nameof(Slug), IsUnique = true)]
    public class Forum 
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(125)]
        public string Slug { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<ForumOwner> ForumOwners { get; set; }
        public ICollection<ForumModerator> ForumModerators { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<ForumMember> Members { get; set; }
    }
}
