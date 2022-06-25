using Lucene.Net.Analysis;
using Lucene.Net.Analysis.En;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Queries;
using Lucene.Net.Search;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Timers;
using Directory = Lucene.Net.Store.Directory;
using Timer = System.Timers.Timer;

namespace Api.Services.FullText
{
    public class FullTextIndex : IDisposable, IFullTextIndex
    {
        private readonly Directory directory;
        private readonly Analyzer analyzer;
        private readonly IndexWriter writer;
        private readonly ReferenceManager<IndexSearcher> manager;
        private readonly ControlledRealTimeReopenThread<IndexSearcher> nrt;

        private readonly Timer timer;

        private readonly DocumentConverter converter = new DocumentConverter();

        public FullTextIndex(Directory directory)
        {
            this.directory = directory;
            this.analyzer = new EnglishAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            var config = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyzer)
            {
                OpenMode = OpenMode.CREATE_OR_APPEND,
                UseCompoundFile = false,
                UseReaderPooling = true
            };
            this.writer = new IndexWriter(this.directory, config);

            var trackingIndexWriter = new TrackingIndexWriter(this.writer);
            this.manager = new SearcherManager(this.writer, true, null);

            this.nrt = new ControlledRealTimeReopenThread<IndexSearcher>(trackingIndexWriter, this.manager, 1.0, 0.01);
            this.nrt.Name = "NRT Reopen Thread";
            this.nrt.Priority = System.Threading.Thread.CurrentThread.Priority + 1;
            this.nrt.SetDaemon(true);
            this.nrt.Start();

            this.timer = new Timer();
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Interval = 500;
            this.timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();
            if (this.writer.HasUncommittedChanges())
            {
                this.writer.Commit();
                this.manager.MaybeRefresh();
            }
            this.timer.Start();
        }

        public void Save<TEntity>(TEntity entity, double? boost = null)
        {
            var request = this.ToDocument(entity);
            if (boost.HasValue)
            {
                request.Document.AddDoubleDocValuesField("__boost", boost.Value);
            }            
            this.writer.UpdateDocument(new Term("__id", request.Id), request.Document);
        }

        public void Commit()
        {
            this.writer.Commit();
            this.manager.MaybeRefresh();
        }

        private IndexRequest ToDocument<TEntity>(TEntity entity)
        {
            return this.converter.Convert(entity);
        }

        public void Dispose()
        {
            this.timer.Dispose();
            this.manager.Dispose();
            this.writer.Dispose();
            this.nrt.Dispose();
            this.directory.Dispose();
        }

        public class IndexRequest
        {
            public string Id { get; set; }
            public Document Document { get; set; }
        }

        public class SearchResults<TEntity>
        {
            public int Total { get; set; }
            public TEntity[] Results { get; set; }
        }

        public void Delete<TEntity>(Query query)
        {
            var bq = new BooleanQuery();
            bq.Add(query, Occur.MUST);
            bq.Add(new TermQuery(new Term("__type", typeof(TEntity).ToString())), Occur.MUST);
            this.writer.DeleteDocuments(bq);
        }

        public void Delete<TEntity>(TEntity entity)
        {
            var document = this.converter.Convert(entity);

            var bq = new BooleanQuery();
            bq.Add(new TermQuery(new Term("__id", document.Id)), Occur.MUST);
            bq.Add(new TermQuery(new Term("__type", typeof(TEntity).ToString())), Occur.MUST);
            this.writer.DeleteDocuments(bq);
        }

        public SearchResults<SuggestionModel> Search(Query query, Filter filter, int startAt, int pageSize, int topN, Sort sort)
        {
            var searcher = this.manager.Acquire();
            try
            {
                TopDocs hits = null;

                if (sort == null)
                {
                    hits = searcher.Search(query, filter, topN);
                }
                else
                {
                    hits = searcher.Search(query, filter, topN, sort);
                }

                var entities = hits.ScoreDocs
                    .Skip(startAt)
                    .Take(pageSize)
                    .Select(D => D.Doc)
                    .Select(D => searcher.Doc(D))
                    .Select(D => new { Type = D.Get("__type"), Content = D.Get("__content") })
                    .Select(D => new SuggestionModel
                    {
                        Type = D.Type,
                        Object = JObject.Parse(D.Content)
                    })
                    .ToArray();

                return new SearchResults<SuggestionModel>
                {
                    Total = hits.TotalHits,
                    Results = entities
                };
            }
            finally
            {
                this.manager.Release(searcher);
            }
        }

        public SearchResults<TEntity> Search<TEntity>(Query query, Filter filter, int startAt, int pageSize, int topN, Sort sort = null)
        {
            var searcher = this.manager.Acquire();
            try
            {
                var bq = new BooleanFilter();
                bq.Add(new TermFilter(new Term("__type", typeof(TEntity).ToString())), Occur.MUST);
                if (filter != null)
                {
                    bq.Add(filter, Occur.MUST);
                }

                TopDocs hits = null;

                if (sort == null)
                {
                    hits = searcher.Search(query, bq, topN);
                }
                else
                {
                    hits = searcher.Search(query, bq, topN, sort);
                }

                var totalHits = hits.TotalHits;

                var entities = hits.ScoreDocs
                    .Skip(startAt)
                    .Take(pageSize)
                    .Select(D => D.Doc)
                    .Select(D => searcher.Doc(D).Get("__content"))
                    .Select(D => JsonConvert.DeserializeObject<TEntity>(D))
                    .ToArray();

                return new SearchResults<TEntity>
                {
                    Total = totalHits,
                    Results = entities
                };
            }
            finally
            {
                this.manager.Release(searcher);
            }
        }

        public DocumentEnumerator<TEntity> All<TEntity>()
        {
            return new DocumentEnumerator<TEntity>(this.manager);
        }
    }

    public class SuggestionModel
    {
        public string Type { get; set; }
        public JObject Object { get; set; }
    }
}
