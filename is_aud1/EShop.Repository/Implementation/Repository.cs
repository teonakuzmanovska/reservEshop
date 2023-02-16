using EShop.Domain.DomainModels;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        
        public void Delete(T entity)
        {
            if(entities == null)
            {
                throw new ArgumentNullException();
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public T Get(int id)
        {
            return entities.FirstOrDefault(z => z.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(T entity)
        {
            if (entities == null)
            {
                throw new ArgumentNullException();
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entities == null)
            {
                throw new ArgumentNullException();
            }
            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
