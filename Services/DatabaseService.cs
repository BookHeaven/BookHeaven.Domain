using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BookHeaven.Domain.Services;
public interface IDatabaseService
{
	Task<T?> Get<T>(object id) where T : class;
	Task<T?> GetBy<T>(Expression<Func<T, bool>> filter) where T : class;
	Task<T?> GetIncluding<T>(object id, params Expression<Func<T, object?>>[] includeProperties) where T : class;
	Task<T?> GetByIncluding<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object?>>[] includeProperties) where T : class;
	Task<IEnumerable<T>> GetAll<T>() where T : class;
	Task<IEnumerable<T>> GetAllBy<T>(Expression<Func<T, bool>> filter) where T : class;
	Task<IEnumerable<T>> GetAllIncluding<T>(params Expression<Func<T, object?>>[] includeProperties) where T : class;
	Task<IEnumerable<T>> GetAllByIncluding<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object?>>[] includeProperties) where T : class;
	Task AddOrUpdate<T>(T entity, bool insertOnly = false) where T : class;
	Task SaveChanges();
	Task Delete<T>(object id) where T : class;
}

public class DatabaseService<TC>(TC context) : IDatabaseService
	where TC : DbContext
{
	public async Task<T?> Get<T>(object id) where T : class
	{
		var idName = typeof(T).Name + "Id";
		return await GetBy<T>(x => EF.Property<object>(x, idName).Equals(id));
	}

	public async Task<T?> GetBy<T>(Expression<Func<T, bool>> filter) where T : class
	{
		return await context.Set<T>().FirstOrDefaultAsync(filter);
	}

	public async Task<T?> GetIncluding<T>(object id, params Expression<Func<T, object?>>[] includeProperties) where T : class
	{
		var idName = typeof(T).Name + "Id";
		return await GetByIncluding(x => EF.Property<object>(x, idName).Equals(id), includeProperties);
	}

	public async Task<T?> GetByIncluding<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object?>>[] includeProperties) where T : class
	{
		var query = context.Set<T>().AsQueryable();
		foreach (var includeProperty in includeProperties)
		{
			query = query.Include(includeProperty);
		}
		return await query.FirstOrDefaultAsync(filter);
	}

	public async Task<IEnumerable<T>> GetAll<T>() where T : class
	{
		return await context.Set<T>().ToListAsync();
	}

	public async Task<IEnumerable<T>> GetAllBy<T>(Expression<Func<T, bool>> filter) where T : class
	{
		return await context.Set<T>().Where(filter).ToListAsync();
	}

	public async Task<IEnumerable<T>> GetAllIncluding<T>(params Expression<Func<T, object?>>[] includeProperties) where T : class
	{
		var query = context.Set<T>().AsQueryable();
		foreach (var includeProperty in includeProperties)
		{
			query = query.Include(includeProperty);
		}
		return await query.ToListAsync();
	}

	public async Task<IEnumerable<T>> GetAllByIncluding<T>(Expression<Func<T, bool>> filter, params Expression<Func<T, object?>>[] includeProperties) where T : class
	{
		var query = context.Set<T>().AsQueryable();
		foreach (var includeProperty in includeProperties)
		{
			query = query.Include(includeProperty);
		}
		return await query.Where(filter).ToListAsync();
	}

	public async Task AddOrUpdate<T>(T entity, bool insertOnly = false) where T : class
	{
		var id = typeof(T).GetProperty(typeof(T).Name + "Id")!.GetValue(entity, null);
		var existingEntity = await context.Set<T>().FindAsync(id);
		if (existingEntity != null)
		{
			if (insertOnly)
				return;
			context.Entry(existingEntity).CurrentValues.SetValues(entity);
		}
		else
		{
			await context.Set<T>().AddAsync(entity);
		}
	}

	public async Task SaveChanges()
	{
		try
		{
			await context.SaveChangesAsync();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			throw;
		}
	}

	public async Task Delete<T>(object id) where T : class
	{
		var entity = await context.Set<T>().FindAsync(id);
		if (entity != null)
		{
			context.Set<T>().Remove(entity);
		}
	}
}