namespace Api.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public User PostedBy { get; set; }
        public int PostedById { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public Comment Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Comment> Children { get; set; }
    }
}
