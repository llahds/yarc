namespace Api.Models
{
    public class ReportQueueItemModel
    {
        public ForumPostListItemModel? Post { get; set; }
        public CommentInfoModel? Comment { get; set; }
        public string[] Reasons { get; set; } = new string[0];
    }
}
