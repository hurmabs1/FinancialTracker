using FinancialTracker.Domain;
using FinancialTracker.Domain.Interfaces;
namespace FinancialTracker.Services
{
    public class AnalyticsService
    {
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<Category> _categoryRepository;

       public  AnalyticsService(IRepository<Transaction> transactionRepository, IRepository<Category> categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }

        public decimal GetTotalIncome()
        {
            return _transactionRepository.GetAll()
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount.Amount);
        }

        public decimal GetTotalExpense()
        {
            return _transactionRepository.GetAll()
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount.Amount);
        }

        public decimal GetBalance()
        {
            return GetTotalIncome() - GetTotalExpense();
        }

        public IReadOnlyList<(string CategoryName, decimal Total)> GetExpensesByCategory()
        {
            var categoryNames = _categoryRepository
                .GetAll()
                .ToDictionary(c => c.Id, c=> c.Name);

            var result = _transactionRepository.GetAll()
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.CategoryId)
                .Select(g => (CategoryName: categoryNames.TryGetValue(g.Key, out var name) ? name : "Неизвестно",
            Total:  g.Sum(t => t.Amount.Amount)))
                .OrderByDescending(x => x.Total)
                .ToList();

            return result;
        }

        public (decimal Income, decimal Expense, decimal Balance) GetSummaryForPeriods(DateTime start, DateTime end)
        {
            var periodTransaction = _transactionRepository.GetAll()
                .Where(t => t.Date >= start && t.Date < end)
                .ToList();
            decimal income = periodTransaction.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount.Amount);
            decimal expense = periodTransaction.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount.Amount);
            return (income, expense, income - expense);

        }

        public IReadOnlyList<(int Id, string CategoryName, decimal Amount, DateTime Date)> GetTopExpenses(int count = 5)
        {
            var categoryNames = _categoryRepository.GetAll().ToDictionary(c => c.Id, c => c.Name);
            var result = _transactionRepository.GetAll()
            .Where(t => t.Type == TransactionType.Expense)
            .OrderByDescending(t => t.Amount.Amount)   // самые дорогие сверху
            .Take(count)                                // только первые N
            .Select(t => (
        t.Id,
        CategoryName: categoryNames.TryGetValue(t.CategoryId, out var name) ? name : "Неизвестно",
        Amount: t.Amount.Amount,
        t.Date
    ))
            .ToList();
            return result;
        }

        public decimal GetAverageDailyExpense()
        {
            var expenses = _transactionRepository.GetAll()
            .Where(t => t.Type == TransactionType.Expense)
            .ToList();

            if (expenses.Count == 0) return 0m;

            decimal totalExpense = expenses.Sum(t => t.Amount.Amount);

            // Количество дней от самой ранней операции до сегодня (включительно)
            DateTime firstExpenseDate = expenses.Min(t => t.Date).Date;  // .Date обнуляет время!
            int daysSpan = (DateTime.Today - firstExpenseDate).Days + 1;

            return totalExpense / daysSpan;
        }
    }
}
