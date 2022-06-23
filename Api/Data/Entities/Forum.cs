using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Api.Data.Entities
{
    [Table("Forums", Schema = "Social")]
    [Index(nameof(Slug), IsUnique = true)]
    [Index(nameof(IsDeleted))]
    public class Forum 
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        [MaxLength(125)]
        public string Slug { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<ForumOwner> ForumOwners { get; set; }
        public ICollection<ForumModerator> ForumModerators { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<ForumMember> Members { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ForumTopic> Topics { get; set; }

        internal string _postSettingsJson { get; set; }

        [NotMapped]
        public ForumPostSettings PostSettings 
        { 
            get
            {
                return JsonSerializer.Deserialize<ForumPostSettings>(string.IsNullOrEmpty(this._postSettingsJson) ? "{}" : this._postSettingsJson);
            } 
            set
            {
                this._postSettingsJson = JsonSerializer.Serialize(value ?? new ForumPostSettings());
            } 
        }
    }
}
