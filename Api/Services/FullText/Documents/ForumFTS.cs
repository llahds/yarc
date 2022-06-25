namespace Api.Services.FullText.Documents
{
    public class ForumFTS
    {
        [Id]
        public int Id { get; set; }
        [TextField]
        public string Name { get; set; }
        [TextField]
        public string Slug { get; set; }
        [TextField]
        public string Description { get; set; }
    }
}
