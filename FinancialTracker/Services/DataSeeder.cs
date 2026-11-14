using FinancialTracker.Domain;
using FinancialTracker.Domain.Interfaces;

namespace FinancialTracker.Services
{
    public static class DataSeeder
    {
        public static void SeedCategories(IRepository<Category> categoryRepository)
        {
            if(categoryRepository.GetAll().Count > 0)
            {
                return;
            }
            categoryRepository.Add (new Category(1, "Продукты", TransactionType.Expense));
            categoryRepository.Add(new Category(2, "Транспорт", TransactionType.Expense));
            categoryRepository.Add(new Category(3, "Жильё", TransactionType.Expense));
            categoryRepository.Add(new Category(4, "Развлечение", TransactionType.Expense));
            categoryRepository.Add(new Category(5, "Прочее", TransactionType.Expense));
            categoryRepository.Add(new Category(6, "Зарплата", TransactionType.Income));
            categoryRepository.Add(new Category(7, "Подарок", TransactionType.Income));
            categoryRepository.Add(new Category(8, "Прочее", TransactionType.Income));







        }
    }
}
