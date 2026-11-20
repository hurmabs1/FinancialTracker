using FinancialTracker.Domain;
using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Storage;


namespace FinancialTracker.Services
{
    public class CategoryService
    {
        private readonly SettingsRepository _settingsRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Transaction> _transactionRepository;

        private int _nextCategoryId;


        public CategoryService(
            IRepository<Category> categoryRepository,
            IRepository<Transaction> transactionRepository,
            SettingsRepository settingsRepository
            )
        {
            _categoryRepository = categoryRepository;
            var _categoryRepositoryItems = categoryRepository.GetAll();
           _transactionRepository = transactionRepository;
            _settingsRepository = settingsRepository;

            int maxExistingId = _categoryRepositoryItems.Count > 0 ? _categoryRepositoryItems.Max(x => x.Id) : 0;

            _nextCategoryId = Math.Max( settingsRepository.GetInt("NextCategoryId", 1),  maxExistingId + 1);
        }

        public IReadOnlyList<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category Add(string name, TransactionType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainValidationException("Имя категории не может быть пустым");
            }

            if (_categoryRepository.GetAll().Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Type == type))
            {
                throw new DomainValidationException("Категория с таким именем и типом уже существует");
            }

            var newCategory = new Category(_nextCategoryId, name, type);

            _nextCategoryId++;
            _settingsRepository.SetInt("NextCategoryId", _nextCategoryId);

            return _categoryRepository.Add(newCategory);
        }
       public void Delete(int id)
        {
            if (_categoryRepository.GetById(id) == null)
            {
                throw new DomainValidationException("Категория с таким Id не найдена");
            }
            if(_transactionRepository.GetAll().Any(x => x.CategoryId == id))
            {
                throw new DomainValidationException("Нельзя удалить категорию, по которой уже есть финансовые операции");

            }

            _categoryRepository.Delete(id);
        }
    }
}
 