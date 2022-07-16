using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Entities
{
    [Index("Code", IsUnique = true)]
    public class ReportReason
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Label { get; set; }
        public ICollection<ReportedPost> ReportedPosts { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
    }
}
