using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Topic
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public ICollection<ForumTopic> Forums { get; set; }
    }
}
