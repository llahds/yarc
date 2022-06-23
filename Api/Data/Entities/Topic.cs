namespace Api.Data.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ForumTopic> Forums { get; set; }
    }
}
