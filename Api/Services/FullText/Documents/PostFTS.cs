namespace Api.Services.FullText.Documents
{
    public class PostFTS
    {
        [Id]
        public int Id { get; set; }
        [TextField]
        public string Title { get; set; }
        [TextField]
        public string Text { get; set; }
        [TextField]
        public string ForumName { get; set; }
    }
}
