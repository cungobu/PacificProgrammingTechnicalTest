using System.Collections.Concurrent;
using System.Transactions;

namespace Domain.Repositories
{
    public class UnitOfWorkTransactionProvider
    {
        private ConcurrentDictionary<IUnitOfWork, UnitOfWorkTransaction> transactions;

        public UnitOfWorkTransactionProvider() 
        {
            this.transactions = new ConcurrentDictionary<IUnitOfWork, UnitOfWorkTransaction>();
        }

        public IUnitOfWorkTransaction GetTransaction(IUnitOfWork unitOfWork, TransactionScopeOption scopeOption)
        {
            switch (scopeOption)
            {
                case TransactionScopeOption.Required:
                case TransactionScopeOption.Suppress:
                    {
                        var transaction = transactions.GetOrAdd(unitOfWork, _ => new UnitOfWorkTransaction(unitOfWork));
                        transaction.AddScope();
                        return transaction;
                    }
                default:
                    throw new NotSupportedException($"Unit Of Work transaction does not support {scopeOption} scope.");
            }
        }
    }
}
