namespace Api.Services.FullText.Documents
{
    public class CommentFTS
    {
        [Id]
        public int Id { get; set; }
        [TextField]
        public string Text { get; set; }
        public int PostId { get; set; }
        public int ForumId { get; set; }
    }
}
