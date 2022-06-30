using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class ClassifyTextRequestModel
    {
        [Required]
        public string Text { get; set; }
    }
}
