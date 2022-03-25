using System.Collections.Generic;

namespace TestUrls.EntityFramework.Repository
{
    internal class UrlRepository<TEntity> : IUrlRepository<TEntity> where TEntity : class
    {
        public void AddRange(IEnumerable<TEntity> models)
        {
            
        }
    }
}
