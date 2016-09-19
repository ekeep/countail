using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GifGenerator.Data
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