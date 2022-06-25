using Lucene.Net.Index;
using Lucene.Net.Queries;
using Lucene.Net.Search;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services.FullText
{
    public class DocumentEnumerator<TEntity> : IEnumerator<JObject>
    {
        private IndexSearcher searcher;
        private Query query;
        private Filter filter;
        private int current = -1;
        private TopDocs hits;

        private readonly ReferenceManager<IndexSearcher> manager;

        public DocumentEnumerator(ReferenceManager<IndexSearcher> manager)
        {
            this.manager = manager;
        }

        public JObject Current { get; private set; }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            this.manager.Release(this.searcher);
        }

        public bool MoveNext()
        {
            this.current++;

            if (this.current == this.hits.ScoreDocs.Length && this.hits.ScoreDocs.Length > 0)
            {
                this.current = 0;
                this.hits = this.searcher.SearchAfter(this.hits.ScoreDocs.Last(), query, filter, 1000);
            }

            if (this.current < this.hits.ScoreDocs.Length)
            {
                this.Current = JObject.Parse(this.searcher.Doc(this.hits.ScoreDocs[this.current].Doc).Get("__content"));
            }

            return this.current < this.hits.ScoreDocs.Length;
        }

        public void Reset()
        {
            this.searcher = this.manager.Acquire();
            this.filter = new TermFilter(new Term("__type", typeof(TEntity).ToString()));
            this.query = new MatchAllDocsQuery();
            this.current = -1;
            this.hits = this.searcher.Search(query, filter, 1000);
        }
    }
}
