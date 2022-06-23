namespace Api.Data.Entities
{
    public class ForumTopic
    {
        public Forum Forum { get; set; }
        public int ForumId { get; set; }
        public Topic Topic { get; set; }
        public int TopicId { get; set; }
    }
}
