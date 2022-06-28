using Microsoft.EntityFrameworkCore;

namespace Api.Data.Entities
{
    [Index(nameof(Hot))]
    [Index(nameof(Top))]
    [Index(nameof(New))]
    [Index(nameof(Rising))]
    public class PostScore
    {
        public Post Post { get; set; }
        public int PostId { get; set; }
        public decimal Hot { get; set; }
        public decimal Top { get; set; }
        public decimal New { get; set; }
        public decimal Rising { get; set; }
    }
}
