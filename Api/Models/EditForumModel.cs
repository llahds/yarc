using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class EditForumModel
    {
        [MaxLength(100), Required]
        public string Name { get; set; }
        
        [MaxLength(4000), Required]
        public string Description { get; set; }
        
        [MaxLength(125), Required]
        public string Slug { get; set; }

        public KeyValueModel[] Topics { get; set; } = new KeyValueModel[0];

        public KeyValueModel[] Moderators { get; set; } = new KeyValueModel[0];

    }
}
