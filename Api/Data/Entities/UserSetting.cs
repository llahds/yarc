using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Entities
{
    [Table("UserSettings", Schema = "Social")]
    [Index(nameof(Key))]
    public class UserSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [MaxLength(25)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
