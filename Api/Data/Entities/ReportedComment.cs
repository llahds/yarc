namespace Api.Data.Entities
{
    public class ReportedComment
    {
        public int Id { get; set; }
        public User ReportedBy { get; set; }
        public int ReportedById { get; set; }
        public Comment Comment { get; set; }
        public int CommentId { get; set; }
        public ReportReason Reason { get; set; }
        public int ReasonId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
