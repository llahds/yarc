using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class EditForumPostModel
    {
        [MaxLength(400), Required]
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
    }
}
