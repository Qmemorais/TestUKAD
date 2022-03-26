using System.Collections.Generic;

namespace TestUrls.EntityFramework.Repository
{
    public class UrlRepository<TEntity> : IUrlRepository<TEntity> where TEntity : class
    {
        private readonly UrlDbContext _context;

        public UrlRepository(UrlDbContext context)
        {
            _context = context;
        }
        public void AddRange(IEnumerable<TEntity> models)
        {
            _context.Set<TEntity>().AddRange(models);
        }
    }
}
