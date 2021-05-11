using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BaseRepository<TEntity>
    where TEntity : class
    {

        protected readonly AppDbContext RepoDbContext;
        protected readonly DbSet<TEntity> RepoDbSet;
        
        
        public BaseRepository(AppDbContext context, DbSet<TEntity> repoDbSet)
        {
            RepoDbContext = context;
            RepoDbSet = repoDbSet;
        }
        
        protected IQueryable<TEntity> CreateQuery(bool noTracking = true)
        {
            var query = RepoDbSet.AsQueryable();

            if (noTracking)
            {
                query = query
                    .AsNoTracking();
            }

            return query;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true)
        {
            var query = CreateQuery(noTracking);

            var res = await query.ToListAsync();
            
            return res!;
            
        }
        
    }
}