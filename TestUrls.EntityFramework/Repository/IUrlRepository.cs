using System.Collections.Generic;

namespace TestUrls.EntityFramework.Repository
{
    public interface IUrlRepository<TEntity> where TEntity : class
    {
        void AddRange(IEnumerable<TEntity> models);
    }
}
