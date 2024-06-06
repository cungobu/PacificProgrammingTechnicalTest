namespace Domain.Repositories
{
    public interface IUnitOfWorkTransaction : IDisposable
    {
        bool IsFinalScope { get; }
        void Commit();
        void Rollback();
    }
}