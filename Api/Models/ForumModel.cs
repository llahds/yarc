namespace Api.Models
{
    public class ForumModel : EditForumModel
    {
        public DateTime CreatedOn { get; set; }
        public int MemberCount { get; set; }
        public bool IsModerator { get; set; }
        public bool IsOwner { get; set; }
        public bool HasJoined { get; set; }
        public bool IsMuted { get; set; }
    }
}
