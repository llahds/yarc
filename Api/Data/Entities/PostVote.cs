using Microsoft.EntityFrameworkCore;

namespace Api.Data.Entities
{
    public class PostVote
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public User By { get; set; }
        public int ById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Vote { get; set; }
    }
}
