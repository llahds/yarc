using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("ForumOwners", Schema = "Social")]
    public class ForumOwner 
    {
        public User Owner { get; set; }
        public int OwnerId { get; set; }
        public Forum Forum { get; set; }
        public int ForumId { get; set; }
    }
}
