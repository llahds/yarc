using Microsoft.EntityFrameworkCore;

namespace Api.Data.Entities
{
    [Index(nameof(Status))]
    public class ForumMember
    {
        public int Id { get; set; }
        public Forum? Forum { get; set; }
        public int ForumId { get; set; }
        public User? Member { get; set; }
        public int MemberId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Status { get; set; }
    }
}
