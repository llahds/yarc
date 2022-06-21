namespace Api.Models
{
    public class ForumPostSettingsModel : ForumPostGuideLinesModel
    {
        public string[] RequiredTitleWords { get; set; } = new string[0];
        public string[] BannedTitleWords { get; set; } = new string[0];
        public string[] PostTextBannedWords { get; set; } = new string[0];
        public bool IsDomainWhitelist { get; set; }
        public string[] Domains { get; set; } = new string[0];
    }
}
