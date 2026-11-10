using FinancialTracker.Domain.Interfaces;

namespace FinancialTracker.Services

{

    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity

    {
        private readonly List<T> _items = new List<T>();


        public IReadOnlyList<T> GetAll()
        {
            return _items.AsReadOnly();
        }

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }
        public T Add(T entity)
        {
            _items.Add(entity);
            return entity;
        }

        public bool Delete(int id)
        {   
            var itemToDelete = GetById(id); 
            if (itemToDelete == null)
            {
                return false;
            }

            _items.Remove(itemToDelete);
            
            return true;
        }





    }

}