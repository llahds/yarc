namespace Api.Models
{
    public class ListResultModel<TEntity>
    {
        public TEntity[] List { get; set; }
        public string SortBy { get; set; }
    }
}
