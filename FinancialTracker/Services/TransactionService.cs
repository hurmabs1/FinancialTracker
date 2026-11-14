using FinancialTracker.Domain;
using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Domain.Interfaces;

namespace FinancialTracker.Services
{
    public class TransactionService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Category> _categoryRepository;

        private int _nextTransactionId;

        public TransactionService(IRepository<Transaction> transactionRepository, IRepository<Category> categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;

            var _transactionRepositoryItems = transactionRepository.GetAll();

            if (_transactionRepositoryItems.Count == 0)
            {
                _nextTransactionId = 1;
            }
            else
            {
                _nextTransactionId = _transactionRepositoryItems.Select(x => x.Id).Max() + 1;
            }
        }

        public IReadOnlyList<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public Transaction Add(DateTime date, TransactionType type, decimal amount, string currency, int categoryId, string? comment)
        {
            var category = _categoryRepository.GetById(categoryId);

            if (category == null)
            {
                throw new DomainValidationException("Выбранная категория не существует");
            }

            if (category.Type != type)
            {
                throw new DomainValidationException($"Тип операции ({type}) не совпадает с типом категории ({category.Type})");
            }

            var money = new Money(amount, currency);
            var newTransaction = new Transaction(_nextTransactionId, categoryId, date, type, money, comment);
            _nextTransactionId++;
            return _transactionRepository.Add(newTransaction);
        }

        public void Delete(int id)
        {
            if(_transactionRepository.GetById(id) == null)
            {
                throw new DomainValidationException("Операция с таким Id не найдена");
            }
            _transactionRepository.Delete(id);
        }
    }
}
