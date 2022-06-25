using Newtonsoft.Json.Linq;

namespace Api.Models
{
    public class SearchGroupResultModel
    {
        public string Type { get; set; }
        public int Total { get; set; }
        public object[] Results { get; set; }
    }
}
