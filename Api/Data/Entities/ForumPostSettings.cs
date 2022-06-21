namespace Api.Data.Entities
{
    public class ForumPostSettings
    {
        public string GuideLines { get; set; }
        public string[] RequiredTitleWords { get; set; } = new string[0];
        public string[] BannedTitleWords { get; set; } = new string[0];
        public string[] PostTextBannedWords { get; set; } = new string[0];
        public bool IsDomainWhitelist { get; set; }
        public string[] Domains { get; set; } = new string[0];
    }
}
