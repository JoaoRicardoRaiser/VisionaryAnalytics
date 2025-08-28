using VisionaryAnalytics.Api.Domain.Entities;

namespace VisionaryAnalytics.Api.Domain.Interfaces;

public interface IRepository<T> where T: EntityBase
{
    Task UpsertAsync(T entity);
}
