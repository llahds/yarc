namespace Api.Models
{
    public class CommentInfoModel : CommentEditModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public PostedByModel PostedBy { get; set; }
    }
}
