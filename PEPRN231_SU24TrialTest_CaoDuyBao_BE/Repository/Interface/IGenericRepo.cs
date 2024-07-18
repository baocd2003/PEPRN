using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public  interface IGenericRepo<T> where T : class
    {
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(object id);
        public IEnumerable<T> GetAll();
        public T? GetById(object id);
        public IQueryable<T> GetQueryable();
    }
}
