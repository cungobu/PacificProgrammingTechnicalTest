using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Infrastructure.Core
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext, new()
    {
        private DbContext dbContext;
        public DbContext DbContext => dbContext ?? (dbContext = dbContextFactory.Init());

        private readonly IDbFactory<TDbContext> dbContextFactory;
        private readonly UnitOfWorkTransactionProvider unitOfWorkTransactionProvider;
        private readonly RepositoryProvider<TDbContext> repositoryProvider;
        public event EventHandler SaveChangeCompleted;

        public UnitOfWork(IDbFactory<TDbContext> dbContextFactory, UnitOfWorkTransactionProvider unitOfWorkTransactionProvider)
        {
            this.dbContextFactory = dbContextFactory;
            this.unitOfWorkTransactionProvider = unitOfWorkTransactionProvider;
            this.repositoryProvider = new RepositoryProvider<TDbContext>(dbContextFactory, this);
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return this.repositoryProvider.GetRepository<TEntity>();            
        }

        #region Implement IUnitOfWork service contract actions
        public void SaveChanges()
        {
            if (this.uowTransaction == null || this.uowTransaction.IsFinalScope)
            {
                this.DbContext.SaveChanges();
                this.IsSavingChanges = false;
                this.SaveChangeCompleted?.Invoke(this, new EventArgs());
            }
            else
            {
                this.IsSavingChanges = true;
            }
        }

        public void Cancel()
        {
            var changedEntries = DbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            this.IsSavingChanges = false;
        }
        #endregion

        #region Implement IDisopsable service contract action
        public void Dispose()
        {
            this.DbContext?.Dispose();
        }
        #endregion

        #region Implementation of UnitOfWork optimistic transaction
        private bool IsSavingChanges { get; set; }

        private IUnitOfWorkTransaction uowTransaction;

        public IUnitOfWorkTransaction BeginTransaction(TransactionScopeOption scope = TransactionScopeOption.Required)
        {
            this.uowTransaction = this.unitOfWorkTransactionProvider.GetTransaction(this, scope);
            return this.uowTransaction;
        }
        public void Finalize()
        {
            if(this.IsSavingChanges)
            {
                this.SaveChanges();
            }
        }
        #endregion
    }
}
