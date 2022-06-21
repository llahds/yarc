namespace Api.Models
{
    public class ReportQueueItemModel
    {
        public ForumPostListItemModel Post { get; set; }
        public string[] Reasons { get; set; }
    }
}
