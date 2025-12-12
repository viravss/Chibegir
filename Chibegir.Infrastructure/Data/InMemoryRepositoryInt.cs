using System.Linq.Expressions;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Common;

namespace Chibegir.Infrastructure.Data;

public class InMemoryRepositoryInt<T> : IRepositoryInt<T> where T : BaseEntityInt
{
    private readonly List<T> _entities = new();
    private int _nextId = 1;

    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = _entities.FirstOrDefault(e => e.Id == id);
        return Task.FromResult<T?>(entity);
    }

    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<T>>(_entities.ToList());
    }

    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var compiled = predicate.Compile();
        var results = _entities.Where(compiled).ToList();
        return Task.FromResult<IEnumerable<T>>(results);
    }

    public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == 0)
        {
            entity.Id = _nextId++;
        }
        _entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var existing = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (existing != null)
        {
            var index = _entities.IndexOf(existing);
            _entities[index] = entity;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = _entities.Any(e => e.Id == id);
        return Task.FromResult(exists);
    }
}

