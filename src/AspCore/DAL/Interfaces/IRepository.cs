using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        IEnumerable<T> Take(int count);
    }
}
