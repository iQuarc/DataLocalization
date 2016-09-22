using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace iQuarc.DataLocalization
{
    public class LocalizedQueryProvider : IQueryProvider
    {
        private readonly IQueryProvider baseProvider;
        private readonly ExpressionVisitor visitor;
        private readonly CultureInfo cultureInfo;

        public LocalizedQueryProvider(IQueryProvider baseProvider, ExpressionVisitor visitor, CultureInfo cultureInfo)
        {
            this.baseProvider = baseProvider;
            this.visitor = visitor;
            this.cultureInfo = cultureInfo;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new LocalizedQueryable<TElement>(baseProvider.CreateQuery<TElement>(expression), cultureInfo);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new LocalizedQueryable(baseProvider.CreateQuery(expression), cultureInfo);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return baseProvider.Execute<TResult>(visitor.Visit(expression));
        }

        public object Execute(Expression expression)
        {
            return baseProvider.Execute(visitor.Visit(expression));
        }
    }
}