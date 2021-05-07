using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //Checks if there is already a hash table
            if (_repositories == null) _repositories = new Hashtable();

            //Checks the name of the TEntity
            var type = typeof(TEntity).Name;

            //Checks if there is already a repository with this type
            if (!_repositories.ContainsKey(type))
            {
                // Create a repository of type generic repository
                var repositoryType = typeof(GenericRepository<>);
                // create an instance of this repository and pass the context 
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                //added to the hash table
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}