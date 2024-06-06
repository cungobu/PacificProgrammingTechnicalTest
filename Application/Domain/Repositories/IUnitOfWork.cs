using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void SaveChanges();
        void Cancel();

        DbContext DbContext { get; }

        #region Support Optimistic Transaction
        IUnitOfWorkTransaction BeginTransaction(TransactionScopeOption scope = TransactionScopeOption.Required);
        void Finalize();
        #endregion

        #region Supported events
        event EventHandler SaveChangeCompleted;
        #endregion
    }
}
