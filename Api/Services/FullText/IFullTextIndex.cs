using Lucene.Net.Search;

namespace Api.Services.FullText
{
    public interface IFullTextIndex
    {
        void Commit();
        void Dispose();
        void Save<TEntity>(TEntity entity, double? boost = null);
        FullTextIndex.SearchResults<TEntity> Search<TEntity>(Query query, Filter filter, int startAt, int pageSize, int topN, Sort sort = null);
        void Delete<TEntity>(Query query);
        void Delete<TEntity>(TEntity entity);
        FullTextIndex.SearchResults<SuggestionModel> Search(Query query, Filter filter, int startAt, int pageSize, int topN, Sort sort);
        DocumentEnumerator<TEntity> All<TEntity>();
    }
}