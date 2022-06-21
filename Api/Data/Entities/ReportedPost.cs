namespace Api.Data.Entities
{
    public class ReportedPost
    {
        public int Id { get; set; }
        public User ReportedBy { get; set; }
        public int ReportedById { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public ReportReason Reason { get; set; }
        public int ReasonId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
