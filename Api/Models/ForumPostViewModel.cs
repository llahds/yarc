namespace Api.Models
{
    public class ForumPostViewModel : ForumPostListItemModel
    {
        public string Text { get; set; } = "";
        public bool CanReport { get; set; }
    }
}
