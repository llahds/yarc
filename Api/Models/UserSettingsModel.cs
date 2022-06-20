using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UserSettingsModel
    {
        [MaxLength(75)]
        public string DisplayName { get; set; }
        [MaxLength(4000)]
        public string About { get; set; }
    }
}
