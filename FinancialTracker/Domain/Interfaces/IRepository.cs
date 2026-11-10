namespace FinancialTracker.Domain.Interfaces

{
    public interface IRepository<T> where T : class
    {
        IReadOnlyList<T> GetAll();
        
        T? GetById(int id);

        T Add(T entity);

        bool Delete(int id);
    }
    
}