using System.Linq;

namespace iQuarc.DataLocalization
{
    public interface ILocalizedQueryable<out T> : IOrderedQueryable<T>
    {
        
    }

    public interface ILocalizedQueryable : IOrderedQueryable
    {
        
    }
}