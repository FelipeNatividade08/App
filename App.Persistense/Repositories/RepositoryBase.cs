using App.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace App.Persistense.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public DbContext _dbcontextEntity { get; set; }
        public DbSet<TEntity> _dbSetEntity { get; set; }

        public RepositoryBase(AppDbContext dbContext)
        {
            _dbcontextEntity = dbContext;
            _dbcontextEntity.ChangeTracker.AutoDetectChangesEnabled = false;
            _dbSetEntity = dbContext.Set<TEntity>();
        }
        public DbContext Context()
        {
            return _dbcontextEntity;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return _dbSetEntity.Where(where).AsNoTracking();
        }

        public void Save(TEntity obj)
        {
            if((Guid)obj.GetType().GetProperty("Id").GetValue(obj, null) != Guid.Empty)
            {
                _dbSetEntity.Update(obj);
            }
            else
            {
                _dbSetEntity.Add(obj);
            }
        }

        public int SaveChanges()
        {
            var written = 0;
            while (written == 0 )
            {
                try
                {
                    written = _dbcontextEntity.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach(var entry in ex.Entries)
                    {
                        throw new NotSupportedException("Ocorreu um erro em: " + entry.Metadata.Name);
                    }
                }
            }
            return written;
        }
        

        public void Update(TEntity obj)
        {
            _dbcontextEntity.Update(obj);
        }
    }
}
