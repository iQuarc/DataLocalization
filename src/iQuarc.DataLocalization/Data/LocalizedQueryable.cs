using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace iQuarc.DataLocalization
{
    internal class LocalizedQueryable<T> : ILocalizedQueryable<T>, IAsyncEnumerable<T>
    {
        private readonly ExpressionVisitor queryRewriteVisitor;
        private readonly IQueryable<T> wrappedQuery;

        public LocalizedQueryable(IQueryable<T> query, CultureInfo cultureInfo)
        {
            wrappedQuery = query;
            Expression = query.Expression;

            queryRewriteVisitor = new LocalizationExpressionVisitor(cultureInfo);
            //Provider = new LocalizedQueryProvider(query.Provider, queryRewriteVisitor, cultureInfo);
            Provider = wrappedQuery.Provider;
            ElementType = typeof(T);
        }

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        {
            return GetRewrittenQuery().ToAsyncEnumerable().GetEnumerator();
        }

        public override string ToString()
        {
            return GetRewrittenQuery().ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetRewrittenQuery().GetEnumerator();
        }

        private IQueryable<T> GetRewrittenQuery()
        {
            return wrappedQuery
                .Provider
                .CreateQuery<T>(queryRewriteVisitor.Visit(wrappedQuery.Expression));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType { get; }

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }
    }

    public class LocalizedQueryable : ILocalizedQueryable
    {
        private readonly IQueryable wrappedQuery;
        private readonly ExpressionVisitor queryRewriteVisitor;

        public LocalizedQueryable(IQueryable query, CultureInfo cultureInfo)
        {
            this.wrappedQuery = query;
            this.Expression = query.Expression;

            this.queryRewriteVisitor = new LocalizationExpressionVisitor(cultureInfo);
            //this.Provider = new LocalizedQueryProvider(query.Provider, this.queryRewriteVisitor, cultureInfo);
            Provider = wrappedQuery.Provider;
            this.ElementType = query.ElementType;
        }

        public Type ElementType { get; }

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public IEnumerator GetEnumerator()
        {
            return
                wrappedQuery.Provider.CreateQuery(this.queryRewriteVisitor.Visit(wrappedQuery.Expression))
                    .GetEnumerator();
        }
    }
}