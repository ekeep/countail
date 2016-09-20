using System.Linq;

namespace Countail.Data.Data
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T FindById(long id);
        IQueryable<T> GetAll();
    }
}