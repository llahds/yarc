namespace Api.Models
{
    public class CommentModel : CommentEditModel
    {
        public int Id { get; set; }
        
        public DateTime CreatedOn { get; set; }
        public PostedByModel PostedBy { get; set; }

        public int ReplyCount { get; set; }
    }
}
