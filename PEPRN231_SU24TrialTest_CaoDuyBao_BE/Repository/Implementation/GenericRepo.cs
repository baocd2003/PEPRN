using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        public WatercolorsPainting2024DBContext _db;
        public DbSet<T> _dbSet;
        public GenericRepo(WatercolorsPainting2024DBContext dbContext)
        {
            _db = dbContext;
            _dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(object id)
        {

            _dbSet.Remove(GetById(id));
            _db.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetById(object id)
        {
            return _dbSet.Find(id);

        }

        public void Update(T entity)
        {
            _dbSet.Update(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }
        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
