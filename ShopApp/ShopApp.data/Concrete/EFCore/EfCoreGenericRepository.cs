using Microsoft.EntityFrameworkCore;
using ShopApp.data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.data.Concrete.EFCore
{
    public class EfCoreGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext context;
        public EfCoreGenericRepository(DbContext ctx)
        {
            context = ctx;
        }
        public void Create(T t)
        {
            
                context.Set<T>().Add(t);
               context.SaveChanges();
            
        }

       

        public void Delete(T entity)
        {
           
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            
        }

        public List<T> GetAll()
        {
           
                return context.Set<T>().ToList();
            
        }

        public virtual T GetById(int id)
        {
                return context.Set<T>().Find(id);
            
        }

        public virtual void Update(T t)
        {
             context.Set<T>().Update(t);
                context.SaveChanges();
            
        }
    }
}
