using System;
using TestUrls.EntityFramework.Models;
using TestUrls.EntityFramework.Repository;

namespace TestUrls.EntityFramework.UnitOfWorkPatern
{
    public interface IUnitOfWork : IDisposable
    {
        IUrlRepository<DbUrlModel> UrlModels { get; }
        IUrlRepository<DbUrlModelResponse> UrlResponseModels { get; }
        void Save();
    }
}
