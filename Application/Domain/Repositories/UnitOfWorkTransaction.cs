namespace Domain.Repositories
{
    public class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private readonly IUnitOfWork unitOfWork;

        private Queue<object> requests = new Queue<object>();

        public UnitOfWorkTransaction(Domain.IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public bool IsFinalScope => requests.Count == 0;

        public void AddScope()
        {
            this.requests.Enqueue(new object());
        }

        public void Dispose()
        {
            // The default action when dispose is commit
            this.Commit();
        }

        public void Commit()
        {
            if (this.requests.Count > 0)
            {
                this.requests.Dequeue();
                this.unitOfWork.Finalize();
            }
        }

        public void Rollback()
        {
            this.unitOfWork.Cancel();
        }
    }
}
