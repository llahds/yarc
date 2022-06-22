namespace Api.Data.Entities
{
    public class CommentVote
    {
        public int Id { get; set; }
        public Comment Comment { get; set; }
        public int CommentId { get; set; }
        public User By { get; set; }
        public int ById { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Vote { get; set; }
    }
}
