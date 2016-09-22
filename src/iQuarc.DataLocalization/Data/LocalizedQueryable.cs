using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace iQuarc.DataLocalization
{
    internal class LocalizedQueryable<T> : ILocalizedQueryable<T>
    {
        private readonly ExpressionVisitor queryRewriteVisitor;
        private readonly IQueryable<T> wrappedQuery;

        public LocalizedQueryable(IQueryable<T> query, CultureInfo cultureInfo)
        {
            wrappedQuery = query;
            Expression = query.Expression;

            queryRewriteVisitor = new LocalizationExpressionVisitor(cultureInfo);
            Provider = new LocalizedQueryProvider(query.Provider, queryRewriteVisitor, cultureInfo);
            ElementType = typeof(T);
        }

        public override string ToString()
        {
            return GetRewritenQuery().ToString();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetRewritenQuery().GetEnumerator();
        }

        private IQueryable<T> GetRewritenQuery()
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
        private readonly ExpressionVisitor translationVisitor;

        public LocalizedQueryable(IQueryable query, CultureInfo cultureInfo)
        {
            this.wrappedQuery = query;
            this.Expression = query.Expression;

            this.translationVisitor = new LocalizationExpressionVisitor(cultureInfo);
            this.Provider = new LocalizedQueryProvider(query.Provider, this.translationVisitor, cultureInfo);
            this.ElementType = query.ElementType;
        }

        public Type ElementType { get; }

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public IEnumerator GetEnumerator()
        {
            return
                wrappedQuery.Provider.CreateQuery(this.translationVisitor.Visit(wrappedQuery.Expression))
                    .GetEnumerator();
        }
    }
}