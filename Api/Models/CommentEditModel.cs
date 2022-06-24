using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CommentEditModel
    {
        [Required]
        public string Text { get; set; } = "";
    }
}
