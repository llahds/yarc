namespace Api.Models
{
    public class CommentModel : CommentInfoModel
    {
        public int ReplyCount { get; set; }
        public int Ups { get; set; }
        public int Downs { get; set; }
        public int? Vote { get; set; }
        public bool CanReport { get; set; }
        public bool CanEdit { get; set; }
    }
}
