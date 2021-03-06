using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class ForumPostListItemModel
    {
        public int Id { get; set; }
        public KeyValueModel? Forum { get; set; }
        public int Ups { get; set; }
        public int Downs { get; set; }
        public PostedByModel? PostedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Title { get; set; } = "";
        public int? Vote { get; set; }
        public int CommentCount { get; set; }
    }
}
