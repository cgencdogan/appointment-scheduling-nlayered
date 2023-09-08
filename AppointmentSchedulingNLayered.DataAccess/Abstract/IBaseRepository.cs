using AppointmentSchedulingNLayered.Entities.Abstract;
using System.Linq.Expressions;

namespace AppointmentSchedulingNLayered.DataAccess.Abstract;
public interface IBaseRepository<T>
    where T : class, IEntity {
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T> GetAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression = null);
}
