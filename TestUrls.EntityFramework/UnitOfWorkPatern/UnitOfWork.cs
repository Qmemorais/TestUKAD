using TestUrls.EntityFramework.Models;
using TestUrls.EntityFramework.Repository;

namespace TestUrls.EntityFramework.UnitOfWorkPatern
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UrlDbContext _context;
        private UrlRepository<DbUrlModel> UrlModel;
        private UrlRepository<DbUrlModelResponse> UrlModelResponse;

        public UnitOfWork(UrlDbContext context)
        {
            _context = context;
        }

        public IUrlRepository<DbUrlModel> UrlModels 
            => UrlModel ??= new UrlRepository<DbUrlModel>(_context);
        public IUrlRepository<DbUrlModelResponse> UrlResponseModels 
            => UrlModelResponse ??= new UrlRepository<DbUrlModelResponse>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
