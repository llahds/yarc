using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities
{
    public class ReportReason
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Label { get; set; }
        public ICollection<ReportedPost> ReportedPosts { get; set; }
    }
}
