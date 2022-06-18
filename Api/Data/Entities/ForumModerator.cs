using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("ForumModerators", Schema = "Social")]
    public class ForumModerator 
    {
        public User Moderator { get; set; }
        public int ModeratorId { get; set; }
        public Forum Forum { get; set; }
        public int ForumId { get; set; }
    }
}
