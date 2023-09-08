using AppointmentSchedulingNLayered.DataAccess.Concrete.EntityFramework.Contexts;
using AppointmentSchedulingNLayered.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppointmentSchedulingNLayered.DataAccess.Abstract.EntityFramework;
public class EFBaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IEntity {
    public EFBaseRepository(MsDbContext context) {
        Context = context;
    }
    protected MsDbContext Context { get; }


    public async Task<TEntity> AddAsync(TEntity entity) {
        var addedEntity = Context.Entry(entity);
        addedEntity.State = EntityState.Added;
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity) {
        var updatedEntity = Context.Entry(entity);
        updatedEntity.State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(TEntity entity) {
        var deletedEntity = Context.Entry(entity);
        deletedEntity.State = EntityState.Deleted;
        await Context.SaveChangesAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression) {
        return await Context.Set<TEntity>().SingleOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> expression = null) {
        return expression == null
            ? await Context.Set<TEntity>().ToListAsync()
            : await Context.Set<TEntity>().Where(expression).ToListAsync();
    }
}
