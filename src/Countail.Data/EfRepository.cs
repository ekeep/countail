using System.Data.Entity;
using System.Linq;
using Countail.Data.Data;


namespace Countail.Data
{
    public class EfRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public EfRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            _context.SaveChanges();
        }

        public virtual T FindById(long id)
        {
            return _dbSet.Single(o => o.Id == id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }
    }
}
